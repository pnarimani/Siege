using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrderHandler : OrderHandler<InspirePeopleOrder>
    {
        const double MoraleGain = 15;
        const double FoodCost = 5;
        const double WaterCost = 5;

        readonly PoliticalState _political;

        public InspirePeopleOrderHandler(InspirePeopleOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            state.Food >= FoodCost && state.Water >= WaterCost && _political.Faith.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);

            state.Food -= FoodCost;
            log.Record("Food", -FoodCost, Order.Id);

            state.Water -= WaterCost;
            log.Record("Water", -WaterCost, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
