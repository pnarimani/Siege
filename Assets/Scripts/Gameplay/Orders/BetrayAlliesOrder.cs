using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BetrayAlliesOrder : IOrder
    {
        const string Narrative = "The deal is struck in a ruined chapel. Thirty crates for thirty names. There is no going back.";
        const double FoodGain = 30;
        const double WaterGain = 30;
        const double MaterialsGain = 20;
        const double UnrestIncrease = 18;
        const double MoraleLoss = 18;
        const float RetaliationChance = 0.15f;
        const double RetaliationUnrest = 8;
        const double RetaliationMoraleLoss = 5;

        readonly IPopupService _popup;
        readonly PoliticalState _political;
        readonly ResourceLedger _ledger;

        public BetrayAlliesOrder(IPopupService popup, PoliticalState political, ResourceLedger ledger)
        {
            _popup = popup;
            _political = political;
            _ledger = ledger;
        }

        public string Id => "betray_allies";
        public string Name => "Betray Allies";
        public string Description => "Sell out allied contacts for a windfall of supplies. Permanent consequences and daily retaliation risk.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
        public bool CanDeactivate => false;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Deposit(ResourceType.Food, FoodGain);
            log.Record("Food", FoodGain, Id);

            _ledger.Deposit(ResourceType.Water, WaterGain);
            log.Record("Water", WaterGain, Id);

            _ledger.Deposit(ResourceType.Materials, MaterialsGain);
            log.Record("Materials", MaterialsGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            if (Random.value < RetaliationChance)
            {
                state.Unrest += RetaliationUnrest;
                log.Record("Unrest", RetaliationUnrest, Id + "_retaliation");

                state.Morale -= RetaliationMoraleLoss;
                log.Record("Morale", -RetaliationMoraleLoss, Id + "_retaliation");
            }
        }

        public IOrder Clone() => new BetrayAlliesOrder(_popup, _political, _ledger);
    }
}
