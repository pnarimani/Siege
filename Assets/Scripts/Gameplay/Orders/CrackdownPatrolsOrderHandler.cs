using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class CrackdownPatrolsOrderHandler : IOrderHandler
    {
        const double UnrestReduction = 25;
        const int Deaths = 3;
        const double MoraleLoss = 15;
        const double UnrestThreshold = 40;

        readonly CrackdownPatrolsOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public CrackdownPatrolsOrderHandler(CrackdownPatrolsOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Unrest > UnrestThreshold && _political.Tyranny.Value >= 1;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, _order.Id);
            log.Record("Deaths", Deaths, _order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
