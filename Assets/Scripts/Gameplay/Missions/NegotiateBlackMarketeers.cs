using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NegotiateBlackMarketeers : Mission
    {
        const int Duration = 3;
        const int Workers = 2;
        const float ChanceWater = 0.45f;
        const float ChanceFood = 0.30f;
        const double WaterGain = 60;
        const double FoodGain = 50;
        const double DealUnrest = 10;
        const int BetrayalDeaths = 2;
        const double BetrayalUnrest = 25;

        public override string Id => "negotiate_black_marketeers";
        public override string Name => "Negotiate with Black Marketeers";
        public override string Description => "Deal with smugglers to secure scarce supplies. Risky.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            float roll = Random.value;

            if (roll < ChanceWater)
            {
                state.AddResource(ResourceType.Water, WaterGain);
                state.Unrest += DealUnrest;
                log.Record("Water", WaterGain, Name);
                log.Record("Unrest", DealUnrest, Name);
                return new MissionOutcome
                {
                    NarrativeText = "The smugglers delivered water barrels. The people mutter about favoritism.",
                    Success = true
                };
            }

            if (roll < ChanceWater + ChanceFood)
            {
                state.AddResource(ResourceType.Food, FoodGain);
                state.Unrest += DealUnrest;
                log.Record("Food", FoodGain, Name);
                log.Record("Unrest", DealUnrest, Name);
                return new MissionOutcome
                {
                    NarrativeText = "Grain sacks arrived in the night. Whispers spread about where they came from.",
                    Success = true
                };
            }

            state.Unrest += BetrayalUnrest;
            state.TotalDeaths += BetrayalDeaths;
            state.DeathsToday += BetrayalDeaths;
            log.Record("Unrest", BetrayalUnrest, Name);
            log.Record("Deaths", BetrayalDeaths, Name);

            return new MissionOutcome
            {
                NarrativeText = "It was a trap. The smugglers sold us out to the enemy.",
                Success = false,
                Deaths = BetrayalDeaths
            };
        }
    }
}
