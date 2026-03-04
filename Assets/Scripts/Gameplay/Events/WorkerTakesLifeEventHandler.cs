using System;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WorkerTakesLifeEventHandler : EventHandler<WorkerTakesLifeEvent>
    {
        readonly PoliticalState _political;

        public WorkerTakesLifeEventHandler(WorkerTakesLifeEvent gameEvent, PoliticalState political) : base(gameEvent)
        {
            _political = political;
        }

        public override bool CanTrigger(GameState state) =>
            _political.Humanity.Value < 15 && state.Morale < 30;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 1);
            state.Morale -= 5;
            log.Record("TotalDeaths", 1, Event.Name);
            log.Record("DeathsToday", 1, Event.Name);
            log.Record("HealthyWorkers", -1, Event.Name);
            log.Record("Morale", -5, Event.Name);
        }
    }
}
