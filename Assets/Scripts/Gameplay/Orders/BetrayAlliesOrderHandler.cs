using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BetrayAlliesOrderHandler : IOrderHandler
    {
        const double FoodGain = 30;
        const double WaterGain = 30;
        const double MaterialsGain = 20;
        const double UnrestIncrease = 18;
        const double MoraleLoss = 18;
        const float RetaliationChance = 0.15f;
        const double RetaliationUnrest = 8;
        const double RetaliationMoraleLoss = 5;

        readonly BetrayAlliesOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public BetrayAlliesOrderHandler(BetrayAlliesOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food += FoodGain;
            log.Record("Food", FoodGain, _order.Id);

            state.Water += WaterGain;
            log.Record("Water", WaterGain, _order.Id);

            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, _order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, _order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            if (Random.value < RetaliationChance)
            {
                state.Unrest += RetaliationUnrest;
                log.Record("Unrest", RetaliationUnrest, _order.Id + "_retaliation");

                state.Morale -= RetaliationMoraleLoss;
                log.Record("Morale", -RetaliationMoraleLoss, _order.Id + "_retaliation");
            }
        }
    }
}
