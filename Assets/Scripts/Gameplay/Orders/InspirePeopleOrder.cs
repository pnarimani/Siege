using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly PoliticalState _political;
        readonly ResourceLedger _ledger;

        const string Narrative = "A leader climbs the barricade and speaks. For the first time in days, people cheer.";
        const double MoraleGain = 15;
        const double FoodCost = 5;
        const double WaterCost = 5;

        public InspirePeopleOrder(IPopupService popup, PoliticalState political, ResourceLedger ledger)
        {
            _popup = popup;
            _political = political;
            _ledger = ledger;
        }

        public string Id => "inspire_people";
        public string Name => "Inspire the People";
        public string Description => "Spend food and water to rally the populace, lifting spirits significantly.";
        public int CooldownDays => 4;

        public bool CanIssue(GameState state) =>
            _ledger.Has(ResourceType.Food, FoodCost) && _ledger.Has(ResourceType.Water, WaterCost) && _political.Faith.Value >= 2;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            _ledger.Withdraw(ResourceType.Food, FoodCost);
            log.Record("Food", -FoodCost, Id);

            _ledger.Withdraw(ResourceType.Water, WaterCost);
            log.Record("Water", -WaterCost, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new InspirePeopleOrder(_popup, _political, _ledger);
    }
}
