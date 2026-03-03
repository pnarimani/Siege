using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class FoodConfiscationLaw : Law
    {
        const double FoodThreshold = 60;
        const double ImmediateFood = 35;
        const double ImmediateUnrest = 25;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 3;
        const double DailyUnrest = 2;

        public override string Id => "food_confiscation";
        public override string Name => "Food Confiscation";
        public override string Description => "Seize food hoards from the populace. Yields supplies but provokes violence.";
        public override string NarrativeText => "The soldiers broke down the baker's door. His family watched.";

        public override bool CanEnact(GameState state) => state.Food < FoodThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Food += ImmediateFood;
            log.Record("Food", ImmediateFood, "Food Confiscation");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Food Confiscation");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Food Confiscation");

            state.HealthyWorkers -= ImmediateDeaths;
            state.TotalDeaths += ImmediateDeaths;
            state.DeathsToday += ImmediateDeaths;
            log.Record("HealthyWorkers", -ImmediateDeaths, "Food Confiscation (violence)");
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Food Confiscation");
        }
    }
}
