using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SearchAbandonedHomesHandler : MissionHandler<SearchAbandonedHomes>
    {
        const int Workers = 4;
        const float ChanceMaterials = 0.45f;
        const float ChanceMedicine = 0.35f;
        const double MaterialsGain = 40;
        const double MedicineGain = 25;
        const double SicknessMinor = 5;
        const double SicknessMajor = 15;
        const int FailDeaths = 2;

        public SearchAbandonedHomesHandler(SearchAbandonedHomes mission, IPopupService popup) : base(mission, popup) { }

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Mission.WorkerCost;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceMaterials)
            {
                state.AddResource(ResourceType.Materials, MaterialsGain);
                state.Sickness += SicknessMinor;
                log.Record("Materials", MaterialsGain, Mission.Name);
                log.Record("Sickness", SicknessMinor, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Useful materials recovered, but some workers fell ill from the filth.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceMaterials + ChanceMedicine)
            {
                state.AddResource(ResourceType.Medicine, MedicineGain);
                state.Sickness += SicknessMinor;
                log.Record("Medicine", MedicineGain, Mission.Name);
                log.Record("Sickness", SicknessMinor, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "A hidden apothecary cache. The workers cough but carry on.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Sickness += SicknessMajor;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Sickness", SicknessMajor, Mission.Name);
            log.Record("Deaths", FailDeaths, Mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The houses were plague-ridden. Two workers never returned.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
