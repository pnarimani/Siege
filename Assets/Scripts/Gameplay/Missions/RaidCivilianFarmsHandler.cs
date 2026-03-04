using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class RaidCivilianFarmsHandler : MissionHandler<RaidCivilianFarms>
    {
        const int Workers = 4;
        const float ChanceCleanSuccess = 0.60f;
        const double CleanFood = 60;
        const double DirtyFood = 30;
        const double DirtyUnrest = 15;
        const int DirtyDeaths = 2;

        public RaidCivilianFarmsHandler(RaidCivilianFarms mission, IPopupService popup) : base(mission, popup) { }

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Mission.WorkerCost;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceCleanSuccess)
            {
                state.AddResource(ResourceType.Food, CleanFood);
                log.Record("Food", CleanFood, Mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The farms were abandoned. Carts returned loaded with grain.",
                    Success = true
                };
                Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.AddResource(ResourceType.Food, DirtyFood);
            state.Unrest += DirtyUnrest;
            state.TotalDeaths += DirtyDeaths;
            state.DeathsToday += DirtyDeaths;
            log.Record("Food", DirtyFood, Mission.Name);
            log.Record("Unrest", DirtyUnrest, Mission.Name);
            log.Record("Deaths", DirtyDeaths, Mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "Farmers fought back. We took what we could, but blood was spilled.",
                Success = false,
                Deaths = DirtyDeaths
            };
            Popup.Open(Mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
