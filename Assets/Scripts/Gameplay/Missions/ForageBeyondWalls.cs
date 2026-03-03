using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ForageBeyondWalls : Mission
    {
        const int Duration = 4;
        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.25f;
        const double GreatFood = 80;
        const double PartialFood = 50;
        const int AmbushDeaths = 2;
        const int AmbushWounded = 3;
        const double AmbushUnrest = 10;

        public override string Id => "forage_beyond_walls";
        public override string Name => "Forage Beyond Walls";
        public override string Description => "Send workers to forage for food outside the walls.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            float roll = Random.value;

            if (roll < ChanceGreatSuccess)
            {
                state.AddResource(ResourceType.Food, GreatFood);
                log.Record("Food", GreatFood, Name);
                return new MissionOutcome
                {
                    NarrativeText = "The foragers found a hidden storehouse. A bounty of food returns to the city.",
                    Success = true
                };
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                state.AddResource(ResourceType.Food, PartialFood);
                log.Record("Food", PartialFood, Name);
                return new MissionOutcome
                {
                    NarrativeText = "Slim pickings, but the foragers return with what they could carry.",
                    Success = true
                };
            }

            state.Unrest += AmbushUnrest;
            state.TotalDeaths += AmbushDeaths;
            state.DeathsToday += AmbushDeaths;
            log.Record("Unrest", AmbushUnrest, Name);
            log.Record("Deaths", AmbushDeaths, Name);

            return new MissionOutcome
            {
                NarrativeText = "An enemy patrol ambushed the foragers. Bodies were dragged back through the gate.",
                Success = false,
                Deaths = AmbushDeaths,
                Wounded = AmbushWounded
            };
        }
    }
}
