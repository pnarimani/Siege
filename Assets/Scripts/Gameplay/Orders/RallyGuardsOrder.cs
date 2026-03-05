using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class RallyGuardsOrder : IOrder
    {
        const string Narrative = "Armored boots echo through the square. For a moment, the people remember what order looked like.";
        const double FoodCost = 10;
        const double UnrestReduction = 15;
        const double MoraleGain = 5;
        const int MinGuards = 5;

        readonly IPopupService _popup;

        public RallyGuardsOrder(IPopupService popup) => _popup = popup;

        public string Id => "rally_guards";
        public string Name => "Rally Guards";
        public string Description => "Muster the garrison for a show of force, calming unrest and lifting morale.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Guards >= MinGuards && state.Food >= FoodCost;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new RallyGuardsOrder(_popup);
    }
}
