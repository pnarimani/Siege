using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TotalCollapseEventHandler : IEventHandler
    {
        readonly TotalCollapseEvent _event;

        public string EventId => _event.Id;

        const int DeficitDaysThreshold = 3;

        public TotalCollapseEventHandler(TotalCollapseEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveFoodDeficitDays >= DeficitDaysThreshold &&
            state.ConsecutiveWaterDeficitDays >= DeficitDaysThreshold;
    }
}
