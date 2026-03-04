using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TaintedWellEventHandler : EventHandler<TaintedWellEvent>
    {
        public TaintedWellEventHandler(TaintedWellEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 18;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.AddResource(ResourceType.Water, -20);
            log.Record("Water", -20, Event.Name);
            state.Sickness += 10;
            log.Record("Sickness", 10, Event.Name);
        }
    }
}
