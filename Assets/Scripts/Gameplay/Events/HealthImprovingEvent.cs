using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HealthImprovingEvent : IGameEvent
    {
        const int StreakThreshold = 5;
        const double MoraleBoost = 5.0;
        const double UnrestReduction = 5.0;

        int _lastFiredDay = int.MinValue;

        public string Id => "health_improving";
        public string Name => "Health Improving";
        public string Description => "Days of low sickness restore confidence.";

        public bool CanTrigger(GameState state)
        {
            if (state.ConsecutiveLowSicknessDays >= StreakThreshold
                && state.CurrentDay != _lastFiredDay)
            {
                _lastFiredDay = state.CurrentDay;
                return true;
            }
            return false;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.Morale += MoraleBoost;
            state.Unrest -= UnrestReduction;
            log.Record("Morale", MoraleBoost, Name);
            log.Record("Unrest", -UnrestReduction, Name);
        }

        public string GetNarrativeText(GameState state) =>
            "Days of low sickness have restored confidence.";

        public IGameEvent Clone() => new HealthImprovingEvent();
    }
}
