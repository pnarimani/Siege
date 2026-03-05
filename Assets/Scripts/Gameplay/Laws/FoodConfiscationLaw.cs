using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class FoodConfiscationLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "The soldiers broke down the baker's door. His family watched.";
        const double FoodThreshold = 60;
        const double ImmediateFood = 35;
        const double ImmediateUnrest = 25;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 3;
        const double DailyUnrest = 2;

        public FoodConfiscationLaw(IPopupService popup) => _popup = popup;

        public string Id => "food_confiscation";
        public string Name => "Food Confiscation";
        public string Description => "Seize food hoards from the populace. Yields supplies but provokes violence.";

        public bool CanEnact(GameState state) => state.Food < FoodThreshold;

        public void OnEnact(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Food Confiscation");
        }

        public ILaw Clone() => new FoodConfiscationLaw(_popup);
    }
}
