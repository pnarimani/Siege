using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FeverOutbreakEventHandler : IEventHandler
    {
        readonly FeverOutbreakEvent _event;

        public string EventId => _event.Id;

        public FeverOutbreakEventHandler(FeverOutbreakEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.Sickness > 60;

        public void Execute(GameState state, ChangeLog log)
        {
            int workersLost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= workersLost;
            state.TotalDeaths += 10;
            state.DeathsToday += 10;
            state.Unrest += 10;
            log.Record("HealthyWorkers", -workersLost, _event.Name);
            log.Record("TotalDeaths", 10, _event.Name);
            log.Record("DeathsToday", 10, _event.Name);
            log.Record("Unrest", 10, _event.Name);
        }
    }
}
