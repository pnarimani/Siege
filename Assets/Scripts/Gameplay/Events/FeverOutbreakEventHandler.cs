using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FeverOutbreakEventHandler : EventHandler<FeverOutbreakEvent>
    {
        public FeverOutbreakEventHandler(FeverOutbreakEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.Sickness > 60;

        public override void Execute(GameState state, ChangeLog log)
        {
            int workersLost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= workersLost;
            state.TotalDeaths += 10;
            state.DeathsToday += 10;
            state.Unrest += 10;
            log.Record("HealthyWorkers", -workersLost, Event.Name);
            log.Record("TotalDeaths", 10, Event.Name);
            log.Record("DeathsToday", 10, Event.Name);
            log.Record("Unrest", 10, Event.Name);
        }
    }
}
