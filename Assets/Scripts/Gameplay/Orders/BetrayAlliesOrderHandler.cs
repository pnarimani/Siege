using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BetrayAlliesOrderHandler : OrderHandler<BetrayAlliesOrder>
    {
        const double FoodGain = 30;
        const double WaterGain = 30;
        const double MaterialsGain = 20;
        const double UnrestIncrease = 18;
        const double MoraleLoss = 18;
        const float RetaliationChance = 0.15f;
        const double RetaliationUnrest = 8;
        const double RetaliationMoraleLoss = 5;

        readonly PoliticalState _political;

        public BetrayAlliesOrderHandler(BetrayAlliesOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food += FoodGain;
            log.Record("Food", FoodGain, Order.Id);

            state.Water += WaterGain;
            log.Record("Water", WaterGain, Order.Id);

            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, Order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            if (Random.value < RetaliationChance)
            {
                state.Unrest += RetaliationUnrest;
                log.Record("Unrest", RetaliationUnrest, Order.Id + "_retaliation");

                state.Morale -= RetaliationMoraleLoss;
                log.Record("Morale", -RetaliationMoraleLoss, Order.Id + "_retaliation");
            }
        }
    }
}
