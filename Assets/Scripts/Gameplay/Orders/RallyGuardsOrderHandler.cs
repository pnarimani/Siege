using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RallyGuardsOrderHandler : OrderHandler<RallyGuardsOrder>
    {
        const double FoodCost = 10;
        const double UnrestReduction = 15;
        const double MoraleGain = 5;
        const int MinGuards = 5;

        public RallyGuardsOrderHandler(RallyGuardsOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Guards >= MinGuards && state.Food >= FoodCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
