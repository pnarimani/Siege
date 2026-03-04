using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class FortifyGateOrderHandler : OrderHandler<FortifyGateOrder>
    {
        const double MaterialsCost = 8;
        const double IntegrityGain = 10;
        const double UnrestIncrease = 3;
        const double IntegrityThreshold = 70;

        public FortifyGateOrderHandler(FortifyGateOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost
            && state.Zones[state.ActivePerimeter].Integrity < IntegrityThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Order.Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
