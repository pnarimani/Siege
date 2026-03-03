using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class DivertSuppliesOrder : Order
    {
        const int Cooldown = 3;
        const double MaterialsCost = 20;
        const double FuelCost = 5;

        public override string Id => "divert_supplies";
        public override string Name => "Divert Supplies";
        public override string Description => "Redirect materials and fuel to boost repair output for today.";
        public override string NarrativeText => "Every nail, every plank is accounted for. Today, the builders get everything they need.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && state.Fuel >= FuelCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Id);

            // Repair output boost applied as direct integrity gain to perimeter
            var perimeter = state.ActivePerimeter;
            state.Zones[perimeter].Integrity += 10;
            log.Record("Integrity", 10, Id + " repair boost");
        }
    }
}
