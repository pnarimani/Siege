using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HoldFeastOrder : IOrder
    {
        readonly IPopupService _popup;

        const string Narrative = "For one night, the hall glows warm. Children eat until they are full. Tomorrow the cost will be counted.";
        const double FoodCost = 20;
        const double FuelCost = 10;
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double FoodRequired = 30;

        public HoldFeastOrder(IPopupService popup) => _popup = popup;

        public string Id => "hold_a_feast";
        public string Name => "Hold a Feast";
        public string Description => "A lavish feast burns through supplies but lifts morale and calms unrest.";
        public int CooldownDays => 6;

        public bool CanIssue(GameState state) =>
            state.Food >= FoodRequired && state.Fuel >= FuelCost;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Id);

            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new HoldFeastOrder(_popup);
    }
}
