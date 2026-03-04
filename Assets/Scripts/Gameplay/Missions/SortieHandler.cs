using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SortieHandler : IMissionHandler
    {
        readonly Sortie _mission;
        readonly IPopupService _popup;

        const int GuardsCost = 8;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int GreatIntensityDrop = 1;
        const int FailDeaths = 4;
        const double FailUnrest = 20;

        public SortieHandler(Sortie mission, IPopupService popup)
        {
            _mission = mission;
            _popup = popup;
        }

        public string MissionId => _mission.Id;

        public bool CanLaunch(GameState state) => state.Guards >= _mission.GuardCost;

        public MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - GreatIntensityDrop);
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -GreatIntensityDrop, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "A glorious charge! The enemy lines buckle and their siege stalls.",
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
                    NarrativeText = "The sortie held them at bay. The enemy regroups cautiously.",
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
                NarrativeText = "The sortie was crushed. Survivors limped back, and the city despairs.",
                Success = false,
                Deaths = FailDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
