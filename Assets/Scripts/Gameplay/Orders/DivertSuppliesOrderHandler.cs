using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DivertSuppliesOrderHandler : OrderHandler<DivertSuppliesOrder>
    {
        const double MaterialsCost = 20;
        const double FuelCost = 5;

        public DivertSuppliesOrderHandler(DivertSuppliesOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && state.Fuel >= FuelCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Order.Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Order.Id);

            // Repair output boost applied as direct integrity gain to perimeter
            var perimeter = state.ActivePerimeter;
            state.Zones[perimeter].Integrity += 10;
            log.Record("Integrity", 10, Order.Id + " repair boost");
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
