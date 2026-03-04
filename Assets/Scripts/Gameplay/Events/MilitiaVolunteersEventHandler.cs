using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class MilitiaVolunteersEventHandler : EventHandler<MilitiaVolunteersEvent>
    {
        public MilitiaVolunteersEventHandler(MilitiaVolunteersEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay == 6 && state.HealthyWorkers >= 3;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers -= 3;
                    state.Guards += 3;
                    log.Record("HealthyWorkers", -3, Event.Name);
                    log.Record("Guards", 3, Event.Name);
                    break;
                case 1:
                    state.Morale += 3;
                    log.Record("Morale", 3, Event.Name);
                    break;
                case 2:
                    state.HealthyWorkers -= 5;
                    state.Guards += 5;
                    state.Unrest += 5;
                    log.Record("HealthyWorkers", -5, Event.Name);
                    log.Record("Guards", 5, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
            }
        }
    }
}
