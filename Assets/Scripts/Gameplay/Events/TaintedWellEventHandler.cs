using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TaintedWellEventHandler : IEventHandler
    {
        readonly TaintedWellEvent _event;

        public string EventId => _event.Id;

        public TaintedWellEventHandler(TaintedWellEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 18;

        public void Execute(GameState state, ChangeLog log)
        {
            state.AddResource(ResourceType.Water, -20);
            log.Record("Water", -20, _event.Name);
            state.Sickness += 10;
            log.Record("Sickness", 10, _event.Name);
        }
    }
}
