using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WorkerTakesLifeEvent : IGameEvent
    {
        bool _hasTriggered;
        readonly PoliticalState _political;

        public string Id => "worker_takes_life";
        public string Name => "A Quiet Departure";
        public string Description => "A body is found in the quiet hours. No enemy arrow, no illness — just a soul that could bear no more.";

        public WorkerTakesLifeEvent(PoliticalState political)
        {
            _political = political;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (_political.Humanity.Value < 15 && state.Morale < 30)
            {
                _hasTriggered = true;
                return true;
            }
            return false;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            state.HealthyWorkers = System.Math.Max(0, state.HealthyWorkers - 1);
            state.Morale -= 5;
            log.Record("TotalDeaths", 1, Name);
            log.Record("DeathsToday", 1, Name);
            log.Record("HealthyWorkers", -1, Name);
            log.Record("Morale", -5, Name);
        }

        public IGameEvent Clone() => new WorkerTakesLifeEvent(_political);
    }
}
