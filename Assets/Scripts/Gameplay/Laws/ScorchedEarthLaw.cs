using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLaw : Law
    {
        const double ImmediateMaterials = -20;
        const double ImmediateUnrest = 5;
        const double DailyUnrest = 5;

        public override string Id => "scorched_earth";
        public override string Name => "Scorched Earth";
        public override string Description => "Burn everything the enemy might use. Reduces siege damage but destroys materials and breeds unrest.";
        public override string NarrativeText => "If we cannot hold it, neither shall they. Light the fires.";

        public override double SiegeDamageMultiplier => 0.7;

        // TODO: Add political checks (e.g., Fortification >= threshold) when PoliticalState is accessible
        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Scorched Earth");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Scorched Earth");
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Scorched Earth");
        }
    }
}
