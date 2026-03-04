using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class RefugeesAtGatesEventHandler : EventHandler<RefugeesAtGatesEvent>
    {
        public RefugeesAtGatesEventHandler(RefugeesAtGatesEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 12;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers += 5;
                    state.SickWorkers += 3;
                    state.Unrest += 5;
                    state.Morale += 3;
                    log.Record("HealthyWorkers", 5, Event.Name);
                    log.Record("SickWorkers", 3, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    log.Record("Morale", 3, Event.Name);
                    break;
                case 1:
                    state.HealthyWorkers += 5;
                    state.Unrest += 3;
                    log.Record("HealthyWorkers", 5, Event.Name);
                    log.Record("Unrest", 3, Event.Name);
                    break;
                case 2:
                    state.Morale -= 10;
                    state.Unrest += 5;
                    log.Record("Morale", -10, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
            }
        }
    }
}
