using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FeverOutbreakEvent : GameEvent
    {
        public override string Id => "fever_outbreak";
        public override string Name => "Fever Outbreak";
        public override string Description => "The fever clinic overflows. Bodies are carried out faster than the living can be carried in.";
        public override bool IsOneTime => true;
        public override int Priority => 70;

        public override bool CanTrigger(GameState state)
        {
            return state.Sickness > 60;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            int workersLost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= workersLost;
            state.TotalDeaths += 10;
            state.DeathsToday += 10;
            state.Unrest += 10;

            log.Record("HealthyWorkers", -workersLost, Name);
            log.Record("TotalDeaths", 10, Name);
            log.Record("DeathsToday", 10, Name);
            log.Record("Unrest", 10, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
