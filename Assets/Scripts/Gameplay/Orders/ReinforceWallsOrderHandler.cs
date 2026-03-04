using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ReinforceWallsOrderHandler : OrderHandler<ReinforceWallsOrder>
    {
        const double MaterialsCost = 15;
        const double IntegrityGain = 15;

        readonly PoliticalState _political;

        public ReinforceWallsOrderHandler(ReinforceWallsOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && _political.Fortification.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Order.Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
