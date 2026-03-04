using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicTrialOrderHandler : OrderHandler<PublicTrialOrder>
    {
        const int Deaths = 2;
        const double UnrestReduction = 5;
        const double MoraleLoss = 10;

        readonly PoliticalState _political;

        public PublicTrialOrderHandler(PublicTrialOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 2 || _political.Faith.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Order.Id);
            log.Record("Deaths", Deaths, Order.Id);

            // Tyranny path default
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
