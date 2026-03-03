using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class AbandonOuterRingLaw : Law
    {
        const double IntegrityThreshold = 40;
        const double UnrestIncrease = 15;

        public override string Id => "abandon_outer_ring";
        public override string Name => "Abandon the Outer Ring";
        public override string Description => "Withdraw all forces from the outer farms, ceding them to the enemy. Reduces siege pressure but causes unrest.";
        public override string NarrativeText => "The outer fields are lost. Pull everyone back behind the second wall — and pray it holds.";

        public override double SiegeDamageMultiplier => 0.8;

        public override bool CanEnact(GameState state)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            return !zone.IsLost && zone.Integrity < IntegrityThreshold;
        }

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("Zone", 0, "OuterFarms lost (Abandon Outer Ring)");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Abandon Outer Ring");
        }
    }
}
