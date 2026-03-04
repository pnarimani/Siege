using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SabotageEnemySuppliesHandler : IMissionHandler
    {
        readonly SabotageEnemySupplies _mission;
        readonly IPopupService _popup;

        const int Workers = 4;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 20;

        public SabotageEnemySuppliesHandler(SabotageEnemySupplies mission, IPopupService popup)
        {
            _mission = mission;
            _popup = popup;
        }

        public string MissionId => _mission.Id;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= _mission.WorkerCost;

        public MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 5;
                state.SiegeDamageReductionMultiplier = 0.6;
                log.Record("SiegeIntensity", -1, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Their grain stores are ash. The siege falters as hunger gnaws at the enemy.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, _mission.Name + " (partial)");
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.8;
                outcome = new MissionOutcome
                {
                    NarrativeText = "Some supplies burned, but the enemy recovered quickly. A brief reprieve.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, _mission.Name);
            log.Record("Deaths", FailDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The saboteurs were caught and executed in view of the walls.",
                Success = false,
                Deaths = FailDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
