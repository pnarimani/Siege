using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class QuarantineDistrictOrderHandler : IOrderHandler
    {
        const double SicknessReduction = 12;
        const double UnrestReduction = 3;
        const double SicknessThreshold = 30;

        readonly QuarantineDistrictOrder _order;
        readonly IPopupService _popup;

        public QuarantineDistrictOrderHandler(QuarantineDistrictOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, _order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
