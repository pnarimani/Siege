using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallsStillStandEventHandler : IEventHandler
    {
        readonly WallsStillStandEvent _event;

        public string EventId => _event.Id;

        const int StreakThreshold = 7;
        const double MoraleBoost = 8.0;

        int _lastFiredDay = int.MinValue;

        public WallsStillStandEventHandler(WallsStillStandEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveZoneHeldDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            log.Record("Morale", MoraleBoost, _event.Name);
        }
    }
}
