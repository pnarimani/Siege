using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const string Narrative = "A leader climbs the barricade and speaks. For the first time in days, people cheer.";
        const double MoraleGain = 15;
        const double FoodCost = 5;
        const double WaterCost = 5;

        public InspirePeopleOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "inspire_people";
        public string Name => "Inspire the People";
        public string Description => "Spend food and water to rally the populace, lifting spirits significantly.";
        public int CooldownDays => 4;

        public bool CanIssue(GameState state) =>
            state.Food >= FoodCost && state.Water >= WaterCost && _political.Faith.Value >= 2;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Water -= WaterCost;
            log.Record("Water", -WaterCost, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new InspirePeopleOrder(_popup, _political);
    }
}
