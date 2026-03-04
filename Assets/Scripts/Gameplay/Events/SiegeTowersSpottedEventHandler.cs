using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEventHandler : IEventHandler
    {
        readonly SiegeTowersSpottedEvent _event;

        public string EventId => _event.Id;

        public SiegeTowersSpottedEventHandler(SiegeTowersSpottedEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 7;
    }
}
