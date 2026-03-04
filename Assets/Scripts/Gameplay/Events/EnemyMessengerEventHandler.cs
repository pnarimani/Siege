using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyMessengerEventHandler : IEventHandler
    {
        readonly EnemyMessengerEvent _event;

        public string EventId => _event.Id;

        public EnemyMessengerEventHandler(EnemyMessengerEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 1;
    }
}
