using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class DiplomaticEnvoyHandler : MissionHandler<DiplomaticEnvoy>
    {
        const int Workers = 3;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int GreatDelay = 5;
        const int PartialDelay = 2;
        const int FailDeaths = 3;
        const double FailUnrest = 10;

        public DiplomaticEnvoyHandler(DiplomaticEnvoy mission, IPopupService popup) : base(mission, popup) { }

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
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -1, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The envoys bought time. Rumors say a relief force stirs in the east.",
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
                    NarrativeText = "The enemy entertained our envoys, if only for the amusement. A small delay.",
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
                NarrativeText = "The envoys were hanged from the siege towers. A grim message.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
