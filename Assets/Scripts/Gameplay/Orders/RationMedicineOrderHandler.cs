using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RationMedicineOrderHandler : OrderHandler<RationMedicineOrder>
    {
        const double MedicineCost = 8;
        const double SicknessReduction = 15;
        const double UnrestIncrease = 5;
        const double SicknessThreshold = 20;

        public RationMedicineOrderHandler(RationMedicineOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold && state.Medicine >= MedicineCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine -= MedicineCost;
            log.Record("Medicine", -MedicineCost, Order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
