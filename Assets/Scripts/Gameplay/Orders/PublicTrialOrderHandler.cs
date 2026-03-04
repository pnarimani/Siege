using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicTrialOrderHandler : IOrderHandler
    {
        const int Deaths = 2;
        const double UnrestReduction = 5;
        const double MoraleLoss = 10;

        readonly PublicTrialOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public PublicTrialOrderHandler(PublicTrialOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 2 || _political.Faith.Value >= 2;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, _order.Id);
            log.Record("Deaths", Deaths, _order.Id);

            // Tyranny path default
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
