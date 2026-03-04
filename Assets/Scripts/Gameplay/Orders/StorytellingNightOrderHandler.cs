using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class StorytellingNightOrderHandler : OrderHandler<StorytellingNightOrder>
    {
        const double MoraleGain = 8;
        const double MoraleMin = 20;
        const double MoraleMax = 50;

        public StorytellingNightOrderHandler(StorytellingNightOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Morale >= MoraleMin && state.Morale <= MoraleMax;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
