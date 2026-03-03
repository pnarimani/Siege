using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HealthImprovingEvent : GameEvent
    {
        const int StreakThreshold = 5;
        const double MoraleBoost = 5.0;
        const double UnrestReduction = 5.0;

        int _lastFiredDay = int.MinValue;

        public override string Id => "health_improving";
        public override string Name => "Health Improving";
        public override string Description => "Days of low sickness restore confidence.";
        public override int Priority => 20;
        public override bool IsOneTime => false;

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveLowSicknessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            state.Unrest -= UnrestReduction;
            log.Record("Morale", MoraleBoost, Name);
            log.Record("Unrest", -UnrestReduction, Name);
        }

        public override string GetNarrativeText(GameState state) =>
            "Days of low sickness have restored confidence.";
    }
}
