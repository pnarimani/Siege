using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrderHandler : OrderHandler<HostageExchangeOrder>
    {
        const double DailyFoodCost = 4;
        const double DailyMedicineCost = 2;
        const double DailyMoraleLoss = 3;
        const int WorkerGainInterval = 2;
        const int WorkerGain = 1;

        int _dayCounter;

        public HostageExchangeOrderHandler(HostageExchangeOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.ZonesLostCount >= 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _dayCounter = 0;
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Order.Id);

            state.Medicine -= DailyMedicineCost;
            log.Record("Medicine", -DailyMedicineCost, Order.Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Order.Id);

            _dayCounter++;
            if (_dayCounter >= WorkerGainInterval)
            {
                _dayCounter = 0;
                state.HealthyWorkers += WorkerGain;
                log.Record("HealthyWorkers", WorkerGain, Order.Id);
            }
        }
    }
}
