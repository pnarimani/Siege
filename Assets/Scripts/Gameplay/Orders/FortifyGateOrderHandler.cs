using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class FortifyGateOrderHandler : IOrderHandler
    {
        const double MaterialsCost = 8;
        const double IntegrityGain = 10;
        const double UnrestIncrease = 3;
        const double IntegrityThreshold = 70;

        readonly FortifyGateOrder _order;
        readonly IPopupService _popup;

        public FortifyGateOrderHandler(FortifyGateOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost
            && state.Zones[state.ActivePerimeter].Integrity < IntegrityThreshold;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, _order.Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, _order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
