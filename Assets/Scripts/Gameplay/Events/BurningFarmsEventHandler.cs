using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BurningFarmsEventHandler : IEventHandler
    {
        readonly BurningFarmsEvent _event;

        public string EventId => _event.Id;

        public BurningFarmsEventHandler(BurningFarmsEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 25;
    }
}
