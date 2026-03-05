using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FeverOutbreakEvent : IGameEvent
    {
        const int SicknessThreshold = 60;
        const int MaxWorkersLost = 10;
        const int Deaths = 10;
        const int UnrestIncrease = 10;

        bool _hasTriggered;

        public string Id => "fever_outbreak";
        public string Name => "Fever Outbreak";
        public string Description => "The fever clinic overflows. Bodies are carried out faster than the living can be carried in.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.Sickness <= SicknessThreshold) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            int workersLost = System.Math.Min(MaxWorkersLost, state.HealthyWorkers);
            state.HealthyWorkers -= workersLost;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            state.Unrest += UnrestIncrease;
            log.Record("HealthyWorkers", -workersLost, Name);
            log.Record("TotalDeaths", Deaths, Name);
            log.Record("DeathsToday", Deaths, Name);
            log.Record("Unrest", UnrestIncrease, Name);
        }

        public string GetNarrativeText(GameState state) => Description;

        public IGameEvent Clone() => new FeverOutbreakEvent();
    }
}
