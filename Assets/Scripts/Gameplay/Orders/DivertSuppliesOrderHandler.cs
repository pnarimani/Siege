using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DivertSuppliesOrderHandler : IOrderHandler
    {
        const double MaterialsCost = 20;
        const double FuelCost = 5;

        readonly DivertSuppliesOrder _order;
        readonly IPopupService _popup;

        public DivertSuppliesOrderHandler(DivertSuppliesOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && state.Fuel >= FuelCost;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, _order.Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, _order.Id);

            // Repair output boost applied as direct integrity gain to perimeter
            var perimeter = state.ActivePerimeter;
            state.Zones[perimeter].Integrity += 10;
            log.Record("Integrity", 10, _order.Id + " repair boost");
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
