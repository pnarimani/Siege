using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ForcedLaborOrderHandler : IOrderHandler
    {
        const double MaterialsGain = 15;
        const double UnrestIncrease = 8;
        const int Deaths = 2;

        readonly ForcedLaborOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public ForcedLaborOrderHandler(ForcedLaborOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            _political.Faith.Value < 4;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, _order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, _order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, _order.Id);
            log.Record("Deaths", Deaths, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
