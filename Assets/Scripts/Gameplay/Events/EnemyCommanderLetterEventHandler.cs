using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyCommanderLetterEventHandler : IEventHandler
    {
        readonly EnemyCommanderLetterEvent _event;

        public string EventId => _event.Id;

        public EnemyCommanderLetterEventHandler(EnemyCommanderLetterEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 15;
    }
}
