using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HealthImprovingEventHandler : EventHandler<HealthImprovingEvent>
    {
        const int StreakThreshold = 5;
        const double MoraleBoost = 5.0;
        const double UnrestReduction = 5.0;

        int _lastFiredDay = int.MinValue;

        public HealthImprovingEventHandler(HealthImprovingEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveLowSicknessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            state.Unrest -= UnrestReduction;
            log.Record("Morale", MoraleBoost, Event.Name);
            log.Record("Unrest", -UnrestReduction, Event.Name);
        }
    }
}
