using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SearchAbandonedHomes : Mission
    {
        const int Duration = 2;
        const int Workers = 4;
        const float ChanceMaterials = 0.45f;
        const float ChanceMedicine = 0.35f;
        const double MaterialsGain = 40;
        const double MedicineGain = 25;
        const double SicknessMinor = 5;
        const double SicknessMajor = 15;
        const int FailDeaths = 2;

        public override string Id => "search_abandoned_homes";
        public override string Name => "Search Abandoned Homes";
        public override string Description => "Scour abandoned dwellings for supplies. Risk of disease.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceMaterials)
            {
                state.AddResource(ResourceType.Materials, MaterialsGain);
                state.Sickness += SicknessMinor;
                log.Record("Materials", MaterialsGain, Name);
                log.Record("Sickness", SicknessMinor, Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Useful materials recovered, but some workers fell ill from the filth.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceMaterials + ChanceMedicine)
            {
                state.AddResource(ResourceType.Medicine, MedicineGain);
                state.Sickness += SicknessMinor;
                log.Record("Medicine", MedicineGain, Name);
                log.Record("Sickness", SicknessMinor, Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "A hidden apothecary cache. The workers cough but carry on.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Sickness += SicknessMajor;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Sickness", SicknessMajor, Name);
            log.Record("Deaths", FailDeaths, Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The houses were plague-ridden. Two workers never returned.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
