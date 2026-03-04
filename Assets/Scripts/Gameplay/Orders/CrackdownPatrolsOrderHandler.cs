using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class CrackdownPatrolsOrderHandler : OrderHandler<CrackdownPatrolsOrder>
    {
        const double UnrestReduction = 25;
        const int Deaths = 3;
        const double MoraleLoss = 15;
        const double UnrestThreshold = 40;

        readonly PoliticalState _political;

        public CrackdownPatrolsOrderHandler(CrackdownPatrolsOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            state.Unrest > UnrestThreshold && _political.Tyranny.Value >= 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Order.Id);
            log.Record("Deaths", Deaths, Order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
