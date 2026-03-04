using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HoldFeastOrderHandler : OrderHandler<HoldFeastOrder>
    {
        const double FoodCost = 20;
        const double FuelCost = 10;
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double FoodRequired = 30;

        public HoldFeastOrderHandler(HoldFeastOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Food >= FoodRequired && state.Fuel >= FuelCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Order.Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
