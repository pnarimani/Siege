using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NightRaidHandler : MissionHandler<NightRaid>
    {
        const int Workers = 6;
        const float ChanceGreatSuccess = 0.30f;
        const float ChancePartialSuccess = 0.40f;
        const int GreatDelay = 3;
        const int PartialDelay = 2;
        const int FailDeaths = 2;
        const int FailWounded = 4;
        const double FailUnrest = 15;

        public NightRaidHandler(NightRaid mission, IPopupService popup) : base(mission, popup) { }

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Mission.WorkerCost;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -1, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raid was devastating. Enemy siege engines burn and their advance stalls.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, Mission.Name + " (delay)");
                state.SiegeDamageReductionDays = 2;
                state.SiegeDamageReductionMultiplier = 0.85;
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raiders caused some disruption. The enemy pauses to regroup.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Mission.Name);
            log.Record("Deaths", FailDeaths, Mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The raid failed. Survivors stumbled back bloodied, and the city's fear grows.",
                Success = false,
                Deaths = FailDeaths,
                Wounded = FailWounded
            };
            Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
