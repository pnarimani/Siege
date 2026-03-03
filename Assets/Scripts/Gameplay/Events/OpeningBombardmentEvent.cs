using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class OpeningBombardmentEvent : GameEvent
    {
        public override string Id => "opening_bombardment";
        public override string Name => "Opening Bombardment";
        public override string Description => "The first boulders crash into Outer Farms. Smoke rises from burning granaries as the enemy demonstrates their intent.";
        public override int Priority => 100;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.Integrity -= 10;
            log.Record("OuterFarms.Integrity", -10, Name);

            state.AddResource(ResourceType.Food, -10);
            log.Record("Food", -10, Name);
        }
    }
}
