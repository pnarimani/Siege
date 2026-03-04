using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class Sortie : Mission
    {
        const int Duration = 1;
        const int GuardsCost = 8;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int GreatIntensityDrop = 1;
        const int FailDeaths = 4;
        const double FailUnrest = 20;

        public override string Id => "sortie";
        public override string Name => "Sortie";
        public override string Description => "Lead guards in a direct assault on enemy positions.";
        public override int DurationDays => Duration;
        public override int WorkerCost => 0;
        public override int GuardCost => GuardsCost;

        public override bool CanLaunch(GameState state) => state.Guards >= GuardsCost;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - GreatIntensityDrop);
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -GreatIntensityDrop, Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "A glorious charge! The enemy lines buckle and their siege stalls.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, Name + " (partial)");
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.8;
                outcome = new MissionOutcome
                {
                    NarrativeText = "The sortie held them at bay. The enemy regroups cautiously.",
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
                NarrativeText = "The sortie was crushed. Survivors limped back, and the city despairs.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
