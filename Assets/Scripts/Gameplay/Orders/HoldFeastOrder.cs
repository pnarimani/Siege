using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class HoldFeastOrder : Order
    {
        const int Cooldown = 6;
        const double FoodCost = 20;
        const double FuelCost = 10;
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double FoodRequired = 30;

        public override string Id => "hold_a_feast";
        public override string Name => "Hold a Feast";
        public override string Description => "A lavish feast burns through supplies but lifts morale and calms unrest.";
        public override string NarrativeText => "For one night, the hall glows warm. Children eat until they are full. Tomorrow the cost will be counted.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Food >= FoodRequired && state.Fuel >= FuelCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);
        }
    }
}
