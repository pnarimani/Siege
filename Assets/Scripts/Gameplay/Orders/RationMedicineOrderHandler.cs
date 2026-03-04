using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RationMedicineOrderHandler : IOrderHandler
    {
        const double MedicineCost = 8;
        const double SicknessReduction = 15;
        const double UnrestIncrease = 5;
        const double SicknessThreshold = 20;

        readonly RationMedicineOrder _order;
        readonly IPopupService _popup;

        public RationMedicineOrderHandler(RationMedicineOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold && state.Medicine >= MedicineCost;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine -= MedicineCost;
            log.Record("Medicine", -MedicineCost, _order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, _order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
