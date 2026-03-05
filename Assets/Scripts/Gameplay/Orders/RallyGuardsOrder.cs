using Siege.Gameplay.Resources;
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
        readonly ResourceLedger _ledger;

        public RallyGuardsOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "rally_guards";
        public string Name => "Rally Guards";
        public string Description => "Muster the garrison for a show of force, calming unrest and lifting morale.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Guards >= MinGuards && _ledger.Has(ResourceType.Food, FoodCost);

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Withdraw(ResourceType.Food, FoodCost);
            log.Record("Food", -FoodCost, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new RallyGuardsOrder(_popup, _ledger);
    }
}
