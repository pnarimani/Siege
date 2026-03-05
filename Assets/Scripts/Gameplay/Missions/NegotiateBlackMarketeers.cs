using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NegotiateBlackMarketeers : IMission
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const int Duration = 3;
        const int Workers = 2;
        const float ChanceWater = 0.45f;
        const float ChanceFood = 0.30f;
        const double WaterGain = 60;
        const double FoodGain = 50;
        const double DealUnrest = 10;
        const int BetrayalDeaths = 2;
        const double BetrayalUnrest = 25;

        const string WaterText = "The smugglers delivered water barrels. The people mutter about favoritism.";
        const string FoodText = "Grain sacks arrived in the night. Whispers spread about where they came from.";
        const string FailText = "It was a trap. The smugglers sold us out to the enemy.";

        int _daysRemaining;
        int _totalDuration;
        int _workersSent;

        public NegotiateBlackMarketeers(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "negotiate_black_marketeers";
        public string Name => "Negotiate with Black Marketeers";
        public string Description => "Deal with smugglers to secure scarce supplies. Risky. Duration: 3d | Workers: 2";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => _totalDuration > 0 ? 1f - (float)_daysRemaining / _totalDuration : 0f;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _totalDuration = Duration;
            _daysRemaining = Duration;
            _workersSent = Workers;
            state.HealthyWorkers -= Workers;
            log.Record("HealthyWorkers", -Workers, Name);
        }

        public void AdvanceDay(GameState state, ChangeLog log) => _daysRemaining--;

        public MissionOutcome Complete(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceWater)
            {
                _ledger.Deposit(ResourceType.Water, WaterGain);
                state.Unrest += DealUnrest;
                log.Record("Water", WaterGain, Name);
                log.Record("Unrest", DealUnrest, Name);
                outcome = new MissionOutcome { NarrativeText = WaterText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceWater + ChanceFood)
            {
                _ledger.Deposit(ResourceType.Food, FoodGain);
                state.Unrest += DealUnrest;
                log.Record("Food", FoodGain, Name);
                log.Record("Unrest", DealUnrest, Name);
                outcome = new MissionOutcome { NarrativeText = FoodText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += BetrayalUnrest;
            state.TotalDeaths += BetrayalDeaths;
            state.DeathsToday += BetrayalDeaths;
            log.Record("Unrest", BetrayalUnrest, Name);
            log.Record("Deaths", BetrayalDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = BetrayalDeaths };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new NegotiateBlackMarketeers(_popup, _ledger);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = Mathf.Max(0, _workersSent - outcome.Deaths - outcome.Wounded);
            if (healthy > 0)
            {
                state.HealthyWorkers += healthy;
                log.Record("HealthyWorkers", healthy, Name + " (returned)");
            }
            if (outcome.Wounded > 0)
            {
                state.SickWorkers += outcome.Wounded;
                log.Record("SickWorkers", outcome.Wounded, Name + " (wounded)");
            }
        }
    }
}
