using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ForageBeyondWalls : IMission
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const int Duration = 4;
        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.25f;
        const double GreatFood = 80;
        const double PartialFood = 50;
        const int AmbushDeaths = 2;
        const int AmbushWounded = 3;
        const double AmbushUnrest = 10;

        const string GreatText = "The foragers found a hidden storehouse. A bounty of food returns to the city.";
        const string PartialText = "Slim pickings, but the foragers return with what they could carry.";
        const string FailText = "An enemy patrol ambushed the foragers. Bodies were dragged back through the gate.";

        int _daysRemaining;
        int _totalDuration;
        int _workersSent;

        public ForageBeyondWalls(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "forage_beyond_walls";
        public string Name => "Forage Beyond Walls";
        public string Description => "Send workers to forage for food outside the walls. Duration: 4d | Workers: 5";
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

            if (roll < ChanceGreatSuccess)
            {
                _ledger.Deposit(ResourceType.Food, GreatFood);
                log.Record("Food", GreatFood, Name);
                outcome = new MissionOutcome { NarrativeText = GreatText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                _ledger.Deposit(ResourceType.Food, PartialFood);
                log.Record("Food", PartialFood, Name);
                outcome = new MissionOutcome { NarrativeText = PartialText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += AmbushUnrest;
            state.TotalDeaths += AmbushDeaths;
            state.DeathsToday += AmbushDeaths;
            log.Record("Unrest", AmbushUnrest, Name);
            log.Record("Deaths", AmbushDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = AmbushDeaths, Wounded = AmbushWounded };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new ForageBeyondWalls(_popup, _ledger);

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
