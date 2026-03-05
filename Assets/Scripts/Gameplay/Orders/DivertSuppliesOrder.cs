using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DivertSuppliesOrder : IOrder
    {
        const string Narrative = "Every nail, every plank is accounted for. Today, the builders get everything they need.";
        const double MaterialsCost = 20;
        const double FuelCost = 5;

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public DivertSuppliesOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "divert_supplies";
        public string Name => "Divert Supplies";
        public string Description => "Redirect materials and fuel to boost repair output for today.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            _ledger.Has(ResourceType.Materials, MaterialsCost) && _ledger.Has(ResourceType.Fuel, FuelCost);

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Withdraw(ResourceType.Materials, MaterialsCost);
            log.Record("Materials", -MaterialsCost, Id);

            _ledger.Withdraw(ResourceType.Fuel, FuelCost);
            log.Record("Fuel", -FuelCost, Id);

            var perimeter = state.ActivePerimeter;
            state.Zones[perimeter].Integrity += 10;
            log.Record("Integrity", 10, Id + " repair boost");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new DivertSuppliesOrder(_popup, _ledger);
    }
}
