using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BetrayAlliesOrder : Order
    {
        const int Cooldown = 0;
        const double FoodGain = 30;
        const double WaterGain = 30;
        const double MaterialsGain = 20;
        const double UnrestIncrease = 18;
        const double MoraleLoss = 18;
        const float RetaliationChance = 0.15f;
        const double RetaliationUnrest = 8;
        const double RetaliationMoraleLoss = 5;

        public override string Id => "betray_allies";
        public override string Name => "Betray Allies";
        public override string Description => "Sell out allied contacts for a windfall of supplies. Permanent consequences and daily retaliation risk.";
        public override string NarrativeText => "The deal is struck in a ruined chapel. Thirty crates for thirty names. There is no going back.";
        public override int CooldownDays => Cooldown;
        public override bool IsToggle => true;
        public override bool CanDeactivate => false;

        public override bool CanIssue(GameState state) =>
            Resolver.Resolve<PoliticalState>().Tyranny.Value >= 4;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food += FoodGain;
            log.Record("Food", FoodGain, Id);

            state.Water += WaterGain;
            log.Record("Water", WaterGain, Id);

            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            if (Random.value < RetaliationChance)
            {
                state.Unrest += RetaliationUnrest;
                log.Record("Unrest", RetaliationUnrest, Id + "_retaliation");

                state.Morale -= RetaliationMoraleLoss;
                log.Record("Morale", -RetaliationMoraleLoss, Id + "_retaliation");
            }
        }
    }
}
