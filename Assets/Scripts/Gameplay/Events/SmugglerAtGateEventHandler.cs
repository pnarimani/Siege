using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SmugglerAtGateEventHandler : EventHandler<SmugglerAtGateEvent>
    {
        public SmugglerAtGateEventHandler(SmugglerAtGateEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 3;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.AddResource(ResourceType.Food, 20);
                    state.AddResource(ResourceType.Materials, -15);
                    log.Record("Food", 20, Event.Name);
                    log.Record("Materials", -15, Event.Name);
                    break;
                case 1:
                    state.AddResource(ResourceType.Food, 30);
                    state.AddResource(ResourceType.Materials, -15);
                    state.Unrest += 5;
                    log.Record("Food", 30, Event.Name);
                    log.Record("Materials", -15, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
            }
        }
    }
}
