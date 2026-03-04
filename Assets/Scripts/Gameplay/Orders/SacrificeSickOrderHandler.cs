using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class SacrificeSickOrderHandler : IOrderHandler
    {
        const int SickRemoved = 3;
        const double SicknessReduction = 8;
        const double UnrestIncrease = 12;
        const double MoraleLoss = 10;
        const int MinSick = 5;

        readonly SacrificeSickOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public SacrificeSickOrderHandler(SacrificeSickOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.SickWorkers > MinSick && _political.Tyranny.Value >= 3;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.SickWorkers -= SickRemoved;
            log.Record("SickWorkers", -SickRemoved, _order.Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, _order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, _order.Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, _order.Id);

            state.TotalDeaths += SickRemoved;
            state.DeathsToday += SickRemoved;
            log.Record("Deaths", SickRemoved, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
