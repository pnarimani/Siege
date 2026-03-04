using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class EngineerTunnelsHandler : MissionHandler<EngineerTunnels>
    {
        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 10;

        public EngineerTunnelsHandler(EngineerTunnels mission, IPopupService popup) : base(mission, popup) { }

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Mission.WorkerCost;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 5;
                state.SiegeDamageReductionMultiplier = 0.6;
                log.Record("SiegeIntensity", -1, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The tunnels collapsed their siege ramp. The enemy scrambles to rebuild.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, Mission.Name + " (partial)");
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.8;
                outcome = new MissionOutcome
                {
                    NarrativeText = "The tunnels disrupted their approach. A partial success.",
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
                NarrativeText = "The tunnels collapsed. Workers are buried alive.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
