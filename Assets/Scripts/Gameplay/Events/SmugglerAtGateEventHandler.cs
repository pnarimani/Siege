using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SmugglerAtGateEventHandler : IEventHandler
    {
        readonly SmugglerAtGateEvent _event;

        public string EventId => _event.Id;

        public SmugglerAtGateEventHandler(SmugglerAtGateEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 3;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.AddResource(ResourceType.Food, 20);
                    state.AddResource(ResourceType.Materials, -15);
                    log.Record("Food", 20, _event.Name);
                    log.Record("Materials", -15, _event.Name);
                    break;
                case 1:
                    state.AddResource(ResourceType.Food, 30);
                    state.AddResource(ResourceType.Materials, -15);
                    state.Unrest += 5;
                    log.Record("Food", 30, _event.Name);
                    log.Record("Materials", -15, _event.Name);
                    log.Record("Unrest", 5, _event.Name);
                    break;
            }
        }
    }
}
