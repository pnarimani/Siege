using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CouncilRevoltEventHandler : IEventHandler
    {
        readonly CouncilRevoltEvent _event;

        public string EventId => _event.Id;

        const double RevoltThreshold = 90.0;

        public CouncilRevoltEventHandler(CouncilRevoltEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.Unrest > RevoltThreshold;
    }
}
