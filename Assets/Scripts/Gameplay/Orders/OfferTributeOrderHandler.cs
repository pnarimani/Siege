using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class OfferTributeOrderHandler : OrderHandler<OfferTributeOrder>
    {
        const double DailyFoodCost = 12;
        const double DailyWaterCost = 12;
        const double DailyMoraleLoss = 6;

        public OfferTributeOrderHandler(OfferTributeOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) => true;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            // Pause siege escalation: reduce intensity by 1 while active
            state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
            log.Record("SiegeIntensity", -1, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Order.Id);

            state.Water -= DailyWaterCost;
            log.Record("Water", -DailyWaterCost, Order.Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Order.Id);
        }
    }
}
