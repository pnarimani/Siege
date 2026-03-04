using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class BurnSurplusOrderHandler : OrderHandler<BurnSurplusOrder>
    {
        const double MaterialsCost = 10;
        const double SicknessReduction = 8;
        const double MoraleGain = 8;

        public BurnSurplusOrderHandler(BurnSurplusOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
