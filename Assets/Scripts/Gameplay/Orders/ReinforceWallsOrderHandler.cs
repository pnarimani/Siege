using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ReinforceWallsOrderHandler : IOrderHandler
    {
        const double MaterialsCost = 15;
        const double IntegrityGain = 15;

        readonly ReinforceWallsOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public ReinforceWallsOrderHandler(ReinforceWallsOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && _political.Fortification.Value >= 2;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, _order.Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
