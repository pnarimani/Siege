using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class OfferTributeOrderHandler : IOrderHandler
    {
        const double DailyFoodCost = 12;
        const double DailyWaterCost = 12;
        const double DailyMoraleLoss = 6;

        readonly OfferTributeOrder _order;
        readonly IPopupService _popup;

        public OfferTributeOrderHandler(OfferTributeOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) => true;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            // Pause siege escalation: reduce intensity by 1 while active
            state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
            log.Record("SiegeIntensity", -1, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, _order.Id);

            state.Water -= DailyWaterCost;
            log.Record("Water", -DailyWaterCost, _order.Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, _order.Id);
        }
    }
}
