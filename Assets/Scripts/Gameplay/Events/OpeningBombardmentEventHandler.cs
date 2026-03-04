using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class OpeningBombardmentEventHandler : EventHandler<OpeningBombardmentEvent>
    {
        public OpeningBombardmentEventHandler(OpeningBombardmentEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.Integrity -= 10;
            log.Record("OuterFarms.Integrity", -10, Event.Name);
            state.AddResource(ResourceType.Food, -10);
            log.Record("Food", -10, Event.Name);
        }
    }
}
