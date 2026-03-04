using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DayOfRemembranceOrderHandler : IOrderHandler
    {
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 5;
        const double MoraleThreshold = 30;

        readonly DayOfRemembranceOrder _order;
        readonly IPopupService _popup;

        public DayOfRemembranceOrderHandler(DayOfRemembranceOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Morale < MoraleThreshold;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, _order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
