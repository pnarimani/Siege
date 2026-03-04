using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EventTriggerSystem : ISimulationSystem
    {
        readonly EventDispatcher _eventDispatcher;

        public EventTriggerSystem(EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            _eventDispatcher.EvaluateEvents(state);
        }
    }
}
