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

        public DivertSuppliesOrder(IPopupService popup)
        {
            _popup = popup;
        }

        public string Id => "divert_supplies";
        public string Name => "Divert Supplies";
        public string Description => "Redirect materials and fuel to boost repair output for today.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && state.Fuel >= FuelCost;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Id);

            var perimeter = state.ActivePerimeter;
            state.Zones[perimeter].Integrity += 10;
            log.Record("Integrity", 10, Id + " repair boost");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new DivertSuppliesOrder(_popup);
    }
}
