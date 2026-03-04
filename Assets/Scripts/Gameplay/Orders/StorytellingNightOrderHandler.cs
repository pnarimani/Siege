using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class StorytellingNightOrderHandler : IOrderHandler
    {
        const double MoraleGain = 8;
        const double MoraleMin = 20;
        const double MoraleMax = 50;

        readonly StorytellingNightOrder _order;
        readonly IPopupService _popup;

        public StorytellingNightOrderHandler(StorytellingNightOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Morale >= MoraleMin && state.Morale <= MoraleMax;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
