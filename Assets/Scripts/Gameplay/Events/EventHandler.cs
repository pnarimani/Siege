using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public abstract class EventHandler<T> : IEventHandler where T : GameEvent
    {
        protected readonly T Event;

        protected EventHandler(T gameEvent)
        {
            Event = gameEvent;
        }

        public string EventId => Event.Id;
        public abstract bool CanTrigger(GameState state);
        public virtual void Execute(GameState state, ChangeLog log) { }
        public virtual void ExecuteResponse(GameState state, ChangeLog log, int responseIndex) { }
    }
}
