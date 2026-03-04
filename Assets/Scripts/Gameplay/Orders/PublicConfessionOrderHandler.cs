using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicConfessionOrderHandler : IOrderHandler
    {
        const double UnrestReduction = 20;
        const double MoraleLoss = 10;
        const int Deaths = 2;

        readonly PublicConfessionOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public PublicConfessionOrderHandler(PublicConfessionOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4 && _political.FearLevel.Value >= 2;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, _order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, _order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, _order.Id);
            log.Record("Deaths", Deaths, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
