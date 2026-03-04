using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class RaidCivilianFarmsHandler : IMissionHandler
    {
        readonly RaidCivilianFarms _mission;
        readonly IPopupService _popup;

        const int Workers = 4;
        const float ChanceCleanSuccess = 0.60f;
        const double CleanFood = 60;
        const double DirtyFood = 30;
        const double DirtyUnrest = 15;
        const int DirtyDeaths = 2;

        public RaidCivilianFarmsHandler(RaidCivilianFarms mission, IPopupService popup)
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

            if (roll < ChanceCleanSuccess)
            {
                state.AddResource(ResourceType.Food, CleanFood);
                log.Record("Food", CleanFood, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The farms were abandoned. Carts returned loaded with grain.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.AddResource(ResourceType.Food, DirtyFood);
            state.Unrest += DirtyUnrest;
            state.TotalDeaths += DirtyDeaths;
            state.DeathsToday += DirtyDeaths;
            log.Record("Food", DirtyFood, _mission.Name);
            log.Record("Unrest", DirtyUnrest, _mission.Name);
            log.Record("Deaths", DirtyDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "Farmers fought back. We took what we could, but blood was spilled.",
                Success = false,
                Deaths = DirtyDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
