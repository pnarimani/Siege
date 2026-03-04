using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NightRaidHandler : IMissionHandler
    {
        readonly NightRaid _mission;
        readonly IPopupService _popup;

        const int Workers = 6;
        const float ChanceGreatSuccess = 0.30f;
        const float ChancePartialSuccess = 0.40f;
        const int GreatDelay = 3;
        const int PartialDelay = 2;
        const int FailDeaths = 2;
        const int FailWounded = 4;
        const double FailUnrest = 15;

        public NightRaidHandler(NightRaid mission, IPopupService popup)
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
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -1, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raid was devastating. Enemy siege engines burn and their advance stalls.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, _mission.Name + " (delay)");
                state.SiegeDamageReductionDays = 2;
                state.SiegeDamageReductionMultiplier = 0.85;
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raiders caused some disruption. The enemy pauses to regroup.",
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
                NarrativeText = "The raid failed. Survivors stumbled back bloodied, and the city's fear grows.",
                Success = false,
                Deaths = FailDeaths,
                Wounded = FailWounded
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
