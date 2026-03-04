using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicConfessionOrderHandler : OrderHandler<PublicConfessionOrder>
    {
        const double UnrestReduction = 20;
        const double MoraleLoss = 10;
        const int Deaths = 2;

        readonly PoliticalState _political;

        public PublicConfessionOrderHandler(PublicConfessionOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4 && _political.FearLevel.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Order.Id);
            log.Record("Deaths", Deaths, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
