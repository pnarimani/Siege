using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class QuarantineDistrictOrderHandler : OrderHandler<QuarantineDistrictOrder>
    {
        const double SicknessReduction = 12;
        const double UnrestReduction = 3;
        const double SicknessThreshold = 30;

        public QuarantineDistrictOrderHandler(QuarantineDistrictOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
