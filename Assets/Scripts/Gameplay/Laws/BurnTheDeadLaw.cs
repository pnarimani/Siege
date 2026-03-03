using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class BurnTheDeadLaw : Law
    {
        const double SicknessThreshold = 35;
        const double SicknessReduction = -15;
        const double FuelCost = -2;
        const double MoraleCost = -10;

        public override string Id => "burn_the_dead";
        public override string Name => "Burn the Dead";
        public override string Description => "Cremate the fallen to halt disease spread. Costs fuel and damages morale.";
        public override string NarrativeText => "The pyres burn day and night. The smoke chokes the living, but the plague recedes.";

        public override bool CanEnact(GameState state) => state.Sickness > SicknessThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Sickness += SicknessReduction;
            log.Record("Sickness", SicknessReduction, "Burn the Dead");

            state.Fuel += FuelCost;
            log.Record("Fuel", FuelCost, "Burn the Dead");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Burn the Dead");
        }
    }
}
