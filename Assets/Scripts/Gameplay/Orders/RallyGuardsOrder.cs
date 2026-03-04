using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RallyGuardsOrder : Order
    {
        const int Cooldown = 3;
        const double FoodCost = 10;
        const double UnrestReduction = 15;
        const double MoraleGain = 5;
        const int MinGuards = 5;

        public override string Id => "rally_guards";
        public override string Name => "Rally Guards";
        public override string Description => "Muster the garrison for a show of force, calming unrest and lifting morale.";
        public override string NarrativeText => "Armored boots echo through the square. For a moment, the people remember what order looked like.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Guards >= MinGuards && state.Food >= FoodCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
