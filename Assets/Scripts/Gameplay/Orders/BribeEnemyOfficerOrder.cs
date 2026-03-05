using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrder : IOrder
    {
        const string Narrative = "Gold changes hands in the dark. The bombardment eases — but someone may be watching.";
        const double DailyFoodCost = 10;
        const double DailyMaterialsCost = 7;
        const float InterceptChance = 0.10f;
        const double InterceptUnrest = 12;

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public BribeEnemyOfficerOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "bribe_enemy_officer";
        public string Name => "Bribe Enemy Officer";
        public string Description => "Pay a daily tribute to an enemy officer to reduce siege damage. Risk of interception.";
        public int CooldownDays => 0;
        public bool IsToggle => true;

        public bool CanIssue(GameState state) => true;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Food, DailyFoodCost);
            log.Record("Food", -DailyFoodCost, Id);

            _ledger.Withdraw(ResourceType.Materials, DailyMaterialsCost);
            log.Record("Materials", -DailyMaterialsCost, Id);

            state.SiegeDamageReductionDays = 1;
            state.SiegeDamageReductionMultiplier = 0.8;

            if (Random.value < InterceptChance)
            {
                state.Unrest += InterceptUnrest;
                log.Record("Unrest", InterceptUnrest, Id + "_intercepted");
            }
        }

        public IOrder Clone() => new BribeEnemyOfficerOrder(_popup, _ledger);
    }
}
