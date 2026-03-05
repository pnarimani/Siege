using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const string Narrative = "A figure stumbles through the gate, gaunt and shaking. One more mouth to feed. One more soul saved.";
        const double DailyFoodCost = 4;
        const double DailyMedicineCost = 2;
        const double DailyMoraleLoss = 3;
        const int WorkerGainInterval = 2;
        const int WorkerGain = 1;

        int _dayCounter;

        public HostageExchangeOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "hostage_exchange";
        public string Name => "Hostage Exchange";
        public string Description => "Trade supplies to recover captured citizens. A slow, costly process.";
        public int CooldownDays => 0;
        public bool IsToggle => true;

        public bool CanIssue(GameState state) =>
            state.ZonesLostCount >= 1;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _dayCounter = 0;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Food, DailyFoodCost);
            log.Record("Food", -DailyFoodCost, Id);

            _ledger.Withdraw(ResourceType.Medicine, DailyMedicineCost);
            log.Record("Medicine", -DailyMedicineCost, Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Id);

            _dayCounter++;
            if (_dayCounter >= WorkerGainInterval)
            {
                _dayCounter = 0;
                state.HealthyWorkers += WorkerGain;
                log.Record("HealthyWorkers", WorkerGain, Id);
            }
        }

        public IOrder Clone() => new HostageExchangeOrder(_popup, _ledger);
    }
}
