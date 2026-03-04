using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrderHandler : IOrderHandler
    {
        const double DailyFoodCost = 10;
        const double DailyMaterialsCost = 7;
        const float InterceptChance = 0.10f;
        const double InterceptUnrest = 12;

        readonly BribeEnemyOfficerOrder _order;
        readonly IPopupService _popup;

        public BribeEnemyOfficerOrderHandler(BribeEnemyOfficerOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) => true;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, _order.Id);

            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, _order.Id);

            // -20% siege damage via temporal modifier
            state.SiegeDamageReductionDays = 1;
            state.SiegeDamageReductionMultiplier = 0.8;

            if (Random.value < InterceptChance)
            {
                state.Unrest += InterceptUnrest;
                log.Record("Unrest", InterceptUnrest, _order.Id + "_intercepted");
            }
        }
    }
}
