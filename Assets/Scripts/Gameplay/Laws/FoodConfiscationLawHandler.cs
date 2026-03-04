using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class FoodConfiscationLawHandler : LawHandler<FoodConfiscationLaw>
    {
        const double FoodThreshold = 60;
        const double ImmediateFood = 35;
        const double ImmediateUnrest = 25;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 3;
        const double DailyUnrest = 2;

        public FoodConfiscationLawHandler(FoodConfiscationLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => state.Food < FoodThreshold;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
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
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Food Confiscation");
        }
    }
}
