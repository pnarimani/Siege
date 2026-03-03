using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrder : Order
    {
        const int Cooldown = 4;
        const double MoraleGain = 15;
        const double FoodCost = 5;
        const double WaterCost = 5;

        public override string Id => "inspire_people";
        public override string Name => "Inspire the People";
        public override string Description => "Spend food and water to rally the populace, lifting spirits significantly.";
        public override string NarrativeText => "A leader climbs the barricade and speaks. For the first time in days, people cheer.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Food >= FoodCost && state.Water >= WaterCost && Resolver.Resolve<PoliticalState>().Faith.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Water -= WaterCost;
            log.Record("Water", -WaterCost, Id);
        }
    }
}
