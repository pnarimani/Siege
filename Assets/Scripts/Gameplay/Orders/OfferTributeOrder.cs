using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class OfferTributeOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const string Narrative = "Carts of provisions roll out the gate. The enemy takes them without a word. The people watch in silence.";
        const double DailyFoodCost = 12;
        const double DailyWaterCost = 12;
        const double DailyMoraleLoss = 6;

        public OfferTributeOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "offer_tribute";
        public string Name => "Offer Tribute";
        public string Description => "Send food and water to the besiegers to stall their advance. Devastating to morale.";
        public int CooldownDays => 0;
        public bool IsToggle => true;

        public bool CanIssue(GameState state) => true;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
            log.Record("SiegeIntensity", -1, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Food, DailyFoodCost);
            log.Record("Food", -DailyFoodCost, Id);

            _ledger.Withdraw(ResourceType.Water, DailyWaterCost);
            log.Record("Water", -DailyWaterCost, Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Id);
        }

        public IOrder Clone() => new OfferTributeOrder(_popup, _ledger);
    }
}
