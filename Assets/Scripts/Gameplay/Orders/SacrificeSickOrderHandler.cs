using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class SacrificeSickOrderHandler : OrderHandler<SacrificeSickOrder>
    {
        const int SickRemoved = 3;
        const double SicknessReduction = 8;
        const double UnrestIncrease = 12;
        const double MoraleLoss = 10;
        const int MinSick = 5;

        readonly PoliticalState _political;

        public SacrificeSickOrderHandler(SacrificeSickOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            state.SickWorkers > MinSick && _political.Tyranny.Value >= 3;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.SickWorkers -= SickRemoved;
            log.Record("SickWorkers", -SickRemoved, Order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Order.Id);

            state.TotalDeaths += SickRemoved;
            state.DeathsToday += SickRemoved;
            log.Record("Deaths", SickRemoved, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
