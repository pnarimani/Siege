using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DistributeLuxuriesOrderHandler : IOrderHandler
    {
        const double FuelCost = 5;
        const double MaterialsCost = 5;
        const double MoraleGain = 10;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 3;
        const double MaterialsRequired = 20;
        const double FuelRequired = 20;

        readonly DistributeLuxuriesOrder _order;
        readonly IPopupService _popup;

        public DistributeLuxuriesOrderHandler(DistributeLuxuriesOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsRequired && state.Fuel >= FuelRequired;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, _order.Id);

            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, _order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
