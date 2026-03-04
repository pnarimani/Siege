using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrderHandler : OrderHandler<BribeEnemyOfficerOrder>
    {
        const double DailyFoodCost = 10;
        const double DailyMaterialsCost = 7;
        const float InterceptChance = 0.10f;
        const double InterceptUnrest = 12;

        public BribeEnemyOfficerOrderHandler(BribeEnemyOfficerOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) => true;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Order.Id);

            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, Order.Id);

            // -20% siege damage via temporal modifier
            state.SiegeDamageReductionDays = 1;
            state.SiegeDamageReductionMultiplier = 0.8;

            if (Random.value < InterceptChance)
            {
                state.Unrest += InterceptUnrest;
                log.Record("Unrest", InterceptUnrest, Order.Id + "_intercepted");
            }
        }
    }
}
