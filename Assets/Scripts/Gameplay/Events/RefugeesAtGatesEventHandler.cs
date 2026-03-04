using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class RefugeesAtGatesEventHandler : IEventHandler
    {
        readonly RefugeesAtGatesEvent _event;

        public string EventId => _event.Id;

        public RefugeesAtGatesEventHandler(RefugeesAtGatesEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == 12;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers += 5;
                    state.SickWorkers += 3;
                    state.Unrest += 5;
                    state.Morale += 3;
                    log.Record("HealthyWorkers", 5, _event.Name);
                    log.Record("SickWorkers", 3, _event.Name);
                    log.Record("Unrest", 5, _event.Name);
                    log.Record("Morale", 3, _event.Name);
                    break;
                case 1:
                    state.HealthyWorkers += 5;
                    state.Unrest += 3;
                    log.Record("HealthyWorkers", 5, _event.Name);
                    log.Record("Unrest", 3, _event.Name);
                    break;
                case 2:
                    state.Morale -= 10;
                    state.Unrest += 5;
                    log.Record("Morale", -10, _event.Name);
                    log.Record("Unrest", 5, _event.Name);
                    break;
            }
        }
    }
}
