using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TaintedWellEvent : GameEvent
    {
        public override string Id => "tainted_well";
        public override string Name => "Tainted Well";
        public override string Description => "The well-keeper reports a foul smell from the main cistern. By the time the warning spreads, many have already drunk.";
        public override int Priority => 70;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 18;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.AddResource(ResourceType.Water, -20);
            log.Record("Water", -20, Name);

            state.Sickness += 10;
            log.Record("Sickness", 10, Name);
        }
    }
}
