using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrderHandler : IOrderHandler
    {
        const double DailyFoodCost = 4;
        const double DailyMedicineCost = 2;
        const double DailyMoraleLoss = 3;
        const int WorkerGainInterval = 2;
        const int WorkerGain = 1;

        readonly HostageExchangeOrder _order;
        readonly IPopupService _popup;
        int _dayCounter;

        public HostageExchangeOrderHandler(HostageExchangeOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.ZonesLostCount >= 1;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _dayCounter = 0;
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, _order.Id);

            state.Medicine -= DailyMedicineCost;
            log.Record("Medicine", -DailyMedicineCost, _order.Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, _order.Id);

            _dayCounter++;
            if (_dayCounter >= WorkerGainInterval)
            {
                _dayCounter = 0;
                state.HealthyWorkers += WorkerGain;
                log.Record("HealthyWorkers", WorkerGain, _order.Id);
            }
        }
    }
}
