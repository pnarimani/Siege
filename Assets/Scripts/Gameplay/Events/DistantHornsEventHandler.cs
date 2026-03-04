using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DistantHornsEventHandler : IEventHandler
    {
        readonly DistantHornsEvent _event;

        public string EventId => _event.Id;

        public DistantHornsEventHandler(DistantHornsEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 38;
    }
}
