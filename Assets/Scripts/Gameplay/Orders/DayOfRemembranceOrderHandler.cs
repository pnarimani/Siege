using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DayOfRemembranceOrderHandler : OrderHandler<DayOfRemembranceOrder>
    {
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 5;
        const double MoraleThreshold = 30;

        public DayOfRemembranceOrderHandler(DayOfRemembranceOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Morale < MoraleThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
