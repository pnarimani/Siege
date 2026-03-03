using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class PublicExecutionsLaw : Law
    {
        const double UnrestThreshold = 60;
        const double ImmediateUnrest = -25;
        const double ImmediateMorale = -20;
        const int ImmediateDeaths = 5;

        public override string Id => "public_executions";
        public override string Name => "Public Executions";
        public override string Description => "Execute dissidents publicly to restore order. Crushes unrest but shatters morale.";
        public override string NarrativeText => "The crowd watches in silence. Fear is a kind of obedience.";

        public override bool CanEnact(GameState state) => state.Unrest > UnrestThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Public Executions");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Public Executions");

            state.HealthyWorkers -= ImmediateDeaths;
            state.TotalDeaths += ImmediateDeaths;
            state.DeathsToday += ImmediateDeaths;
            log.Record("HealthyWorkers", -ImmediateDeaths, "Public Executions");
        }
    }
}
