using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NightRaid : Mission
    {
        const int Duration = 2;
        const int Workers = 6;
        const float ChanceGreatSuccess = 0.30f;
        const float ChancePartialSuccess = 0.40f;
        const int GreatDelay = 3;
        const int PartialDelay = 2;
        const int FailDeaths = 2;
        const int FailWounded = 4;
        const double FailUnrest = 15;

        public override string Id => "night_raid";
        public override string Name => "Night Raid";
        public override string Description => "Launch a daring night raid against enemy siege works.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

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
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raid was devastating. Enemy siege engines burn and their advance stalls.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, Name + " (delay)");
                state.SiegeDamageReductionDays = 2;
                state.SiegeDamageReductionMultiplier = 0.85;
                outcome = new MissionOutcome
                {
                    NarrativeText = "The raiders caused some disruption. The enemy pauses to regroup.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Name);
            log.Record("Deaths", FailDeaths, Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The raid failed. Survivors stumbled back bloodied, and the city's fear grows.",
                Success = false,
                Deaths = FailDeaths,
                Wounded = FailWounded
            };
            Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
