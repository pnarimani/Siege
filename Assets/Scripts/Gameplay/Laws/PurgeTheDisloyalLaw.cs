using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class PurgeTheDisloyalLaw : Law
    {
        const double ImmediateUnrest = -30;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 8;

        public override string Id => "purge_disloyal";
        public override string Name => "Purge the Disloyal";
        public override string Description => "Root out suspected traitors and make examples of them. Devastating but effective.";
        public override string NarrativeText => "The lists were drawn up at midnight. By dawn, eight cells were empty.";

        // TODO: Add political checks (e.g., Tyranny >= threshold) when PoliticalState is accessible
        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Purge the Disloyal");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Purge the Disloyal");

            state.HealthyWorkers -= ImmediateDeaths;
            state.TotalDeaths += ImmediateDeaths;
            state.DeathsToday += ImmediateDeaths;
            log.Record("HealthyWorkers", -ImmediateDeaths, "Purge the Disloyal");
        }
    }
}
