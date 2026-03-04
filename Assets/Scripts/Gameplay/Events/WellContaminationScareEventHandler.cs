using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WellContaminationScareEventHandler : IEventHandler
    {
        readonly WellContaminationScareEvent _event;

        public string EventId => _event.Id;

        public WellContaminationScareEventHandler(WellContaminationScareEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 5;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    if (state.Medicine >= 5)
                    {
                        state.AddResource(ResourceType.Medicine, -5);
                        log.Record("Medicine", -5, _event.Name);
                    }
                    break;
                case 1:
                    state.Sickness += 1;
                    log.Record("Sickness", 1, _event.Name);
                    break;
                case 2:
                    state.Sickness += 5;
                    log.Record("Sickness", 5, _event.Name);
                    break;
            }
        }
    }
}
