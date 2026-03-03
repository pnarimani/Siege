using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EventTriggerSystem : ISimulationSystem
    {
        readonly EventManager _eventManager;

        public EventTriggerSystem(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            _eventManager.EvaluateEvents(state);
        }
    }
}
