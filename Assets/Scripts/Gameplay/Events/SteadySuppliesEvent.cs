using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SteadySuppliesEvent : IGameEvent
    {
        const int StreakThreshold = 5;
        const double MaxMoraleBoost = 15.0;

        int _lastFiredDay = int.MinValue;

        public string Id => "steady_supplies";
        public string Name => "Steady Supplies";
        public string Description => "Consistent supply flow boosts morale.";

        public bool CanTrigger(GameState state)
        {
            if (state.ConsecutiveNoDeficitDays >= StreakThreshold
                && state.CurrentDay != _lastFiredDay)
            {
                _lastFiredDay = state.CurrentDay;
                return true;
            }
            return false;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            double boost = System.Math.Min(MaxMoraleBoost, state.ConsecutiveNoDeficitDays);
            state.Morale += boost;
            log.Record("Morale", boost, Name);
        }

        public string GetNarrativeText(GameState state) =>
            "The steady flow of food and water lifts spirits across the city.";

        public IGameEvent Clone() => new SteadySuppliesEvent();
    }
}
