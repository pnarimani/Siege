using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HealthImprovingEventHandler : IEventHandler
    {
        readonly HealthImprovingEvent _event;

        public string EventId => _event.Id;

        const int StreakThreshold = 5;
        const double MoraleBoost = 5.0;
        const double UnrestReduction = 5.0;

        int _lastFiredDay = int.MinValue;

        public HealthImprovingEventHandler(HealthImprovingEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveLowSicknessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            state.Unrest -= UnrestReduction;
            log.Record("Morale", MoraleBoost, _event.Name);
            log.Record("Unrest", -UnrestReduction, _event.Name);
        }
    }
}
