using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class RaidCivilianFarms : Mission
    {
        const int Duration = 2;
        const int Workers = 4;
        const float ChanceCleanSuccess = 0.60f;
        const double CleanFood = 60;
        const double DirtyFood = 30;
        const double DirtyUnrest = 15;
        const int DirtyDeaths = 2;

        public override string Id => "raid_civilian_farms";
        public override string Name => "Raid Civilian Farms";
        public override string Description => "Take food from farms outside the walls. The farmers won't be happy.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            float roll = Random.value;

            if (roll < ChanceCleanSuccess)
            {
                state.AddResource(ResourceType.Food, CleanFood);
                log.Record("Food", CleanFood, Name);
                return new MissionOutcome
                {
                    NarrativeText = "The farms were abandoned. Carts returned loaded with grain.",
                    Success = true
                };
            }

            state.AddResource(ResourceType.Food, DirtyFood);
            state.Unrest += DirtyUnrest;
            state.TotalDeaths += DirtyDeaths;
            state.DeathsToday += DirtyDeaths;
            log.Record("Food", DirtyFood, Name);
            log.Record("Unrest", DirtyUnrest, Name);
            log.Record("Deaths", DirtyDeaths, Name);

            return new MissionOutcome
            {
                NarrativeText = "Farmers fought back. We took what we could, but blood was spilled.",
                Success = false,
                Deaths = DirtyDeaths
            };
        }
    }
}
