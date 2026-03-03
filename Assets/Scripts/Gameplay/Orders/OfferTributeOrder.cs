using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class OfferTributeOrder : Order
    {
        const int Cooldown = 0;
        const double DailyFoodCost = 12;
        const double DailyWaterCost = 12;
        const double DailyMoraleLoss = 6;

        public override string Id => "offer_tribute";
        public override string Name => "Offer Tribute";
        public override string Description => "Send food and water to the besiegers to stall their advance. Devastating to morale.";
        public override string NarrativeText => "Carts of provisions roll out the gate. The enemy takes them without a word. The people watch in silence.";
        public override int CooldownDays => Cooldown;
        public override bool IsToggle => true;

        public override bool CanIssue(GameState state) => true;

        public override void Execute(GameState state, ChangeLog log)
        {
            // TODO: pause siege escalation while active
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Id);

            state.Water -= DailyWaterCost;
            log.Record("Water", -DailyWaterCost, Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Id);
        }
    }
}
