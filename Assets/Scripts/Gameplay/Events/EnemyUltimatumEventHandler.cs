using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyUltimatumEventHandler : EventHandler<EnemyUltimatumEvent>
    {
        public EnemyUltimatumEventHandler(EnemyUltimatumEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 30;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += 10;
                    state.Unrest += 15;
                    log.Record("Morale", 10, Event.Name);
                    log.Record("Unrest", 15, Event.Name);
                    break;
                case 1:
                    state.Morale -= 5;
                    state.Unrest += 5;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 2);
                    log.Record("Morale", -5, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    log.Record("HealthyWorkers", -2, Event.Name);
                    break;
                case 2:
                    state.Morale -= 15;
                    state.Unrest += 20;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 5);
                    log.Record("Morale", -15, Event.Name);
                    log.Record("Unrest", 20, Event.Name);
                    log.Record("HealthyWorkers", -5, Event.Name);
                    break;
            }
        }
    }
}
