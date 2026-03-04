using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RallyGuardsOrderHandler : IOrderHandler
    {
        const double FoodCost = 10;
        const double UnrestReduction = 15;
        const double MoraleGain = 5;
        const int MinGuards = 5;

        readonly RallyGuardsOrder _order;
        readonly IPopupService _popup;

        public RallyGuardsOrderHandler(RallyGuardsOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Guards >= MinGuards && state.Food >= FoodCost;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, _order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
