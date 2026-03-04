using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HoldFeastOrderHandler : IOrderHandler
    {
        const double FoodCost = 20;
        const double FuelCost = 10;
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double FoodRequired = 30;

        readonly HoldFeastOrder _order;
        readonly IPopupService _popup;

        public HoldFeastOrderHandler(HoldFeastOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Food >= FoodRequired && state.Fuel >= FuelCost;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, _order.Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, _order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
