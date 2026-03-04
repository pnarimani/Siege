using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SearchAbandonedHomesHandler : IMissionHandler
    {
        readonly SearchAbandonedHomes _mission;
        readonly IPopupService _popup;

        const int Workers = 4;
        const float ChanceMaterials = 0.45f;
        const float ChanceMedicine = 0.35f;
        const double MaterialsGain = 40;
        const double MedicineGain = 25;
        const double SicknessMinor = 5;
        const double SicknessMajor = 15;
        const int FailDeaths = 2;

        public SearchAbandonedHomesHandler(SearchAbandonedHomes mission, IPopupService popup)
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

            if (roll < ChanceMaterials)
            {
                state.AddResource(ResourceType.Materials, MaterialsGain);
                state.Sickness += SicknessMinor;
                log.Record("Materials", MaterialsGain, _mission.Name);
                log.Record("Sickness", SicknessMinor, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Useful materials recovered, but some workers fell ill from the filth.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceMaterials + ChanceMedicine)
            {
                state.AddResource(ResourceType.Medicine, MedicineGain);
                state.Sickness += SicknessMinor;
                log.Record("Medicine", MedicineGain, _mission.Name);
                log.Record("Sickness", SicknessMinor, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "A hidden apothecary cache. The workers cough but carry on.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Sickness += SicknessMajor;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Sickness", SicknessMajor, _mission.Name);
            log.Record("Deaths", FailDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The houses were plague-ridden. Two workers never returned.",
                Success = false,
                Deaths = FailDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
