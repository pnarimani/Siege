using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SabotageEnemySupplies : IMission
    {
        readonly IPopupService _popup;

        const int DurationDays = 3;
        const int Workers = 4;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 20;

        const string GreatText = "Their grain stores are ash. The siege falters as hunger gnaws at the enemy.";
        const string PartialText = "Some supplies burned, but the enemy recovered quickly. A brief reprieve.";
        const string FailText = "The saboteurs were caught and executed in view of the walls.";

        int _daysRemaining;
        int _workersSent;

        public SabotageEnemySupplies(IPopupService popup) => _popup = popup;

        public string Id => "sabotage_enemy_supplies";
        public string Name => "Sabotage Enemy Supplies";
        public string Description => "Infiltrate enemy camps and destroy their supply lines. Duration: 3d | Workers: 4";
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

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 5;
                state.SiegeDamageReductionMultiplier = 0.6;
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome { NarrativeText = GreatText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.8;
                log.Record("SiegeIntensity", 0, Name + " (partial)");
                outcome = new MissionOutcome { NarrativeText = PartialText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Name);
            log.Record("Deaths", FailDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = FailDeaths };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new SabotageEnemySupplies(_popup);

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
