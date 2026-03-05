using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class RaidCivilianFarms : IMission
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const int DurationDays = 2;
        const int Workers = 4;
        const float ChanceCleanSuccess = 0.60f;
        const double CleanFood = 60;
        const double DirtyFood = 30;
        const double DirtyUnrest = 15;
        const int DirtyDeaths = 2;

        const string CleanText = "The farms were abandoned. Carts returned loaded with grain.";
        const string DirtyText = "Farmers fought back. We took what we could, but blood was spilled.";

        int _daysRemaining;
        int _workersSent;

        public RaidCivilianFarms(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "raid_civilian_farms";
        public string Name => "Raid Civilian Farms";
        public string Description => "Take food from farms outside the walls. Duration: 2d | Workers: 4";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => 1f - (float)_daysRemaining / DurationDays;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _daysRemaining = DurationDays;
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

            if (roll < ChanceCleanSuccess)
            {
                _ledger.Deposit(ResourceType.Food, CleanFood);
                log.Record("Food", CleanFood, Name);
                outcome = new MissionOutcome { NarrativeText = CleanText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            _ledger.Deposit(ResourceType.Food, DirtyFood);
            state.Unrest += DirtyUnrest;
            state.TotalDeaths += DirtyDeaths;
            state.DeathsToday += DirtyDeaths;
            log.Record("Food", DirtyFood, Name);
            log.Record("Unrest", DirtyUnrest, Name);
            log.Record("Deaths", DirtyDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = DirtyText, Success = false, Deaths = DirtyDeaths };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new RaidCivilianFarms(_popup, _ledger);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = System.Math.Max(0, _workersSent - outcome.Deaths - outcome.Wounded);
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
