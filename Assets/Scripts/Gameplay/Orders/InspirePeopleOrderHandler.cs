using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrderHandler : IOrderHandler
    {
        const double MoraleGain = 15;
        const double FoodCost = 5;
        const double WaterCost = 5;

        readonly InspirePeopleOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public InspirePeopleOrderHandler(InspirePeopleOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Food >= FoodCost && state.Water >= WaterCost && _political.Faith.Value >= 2;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);

            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, _order.Id);

            state.Water -= WaterCost;
            log.Record("Water", -WaterCost, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
