using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class BurnSurplusOrderHandler : IOrderHandler
    {
        const double MaterialsCost = 10;
        const double SicknessReduction = 8;
        const double MoraleGain = 8;

        readonly BurnSurplusOrder _order;
        readonly IPopupService _popup;

        public BurnSurplusOrderHandler(BurnSurplusOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, _order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, _order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
