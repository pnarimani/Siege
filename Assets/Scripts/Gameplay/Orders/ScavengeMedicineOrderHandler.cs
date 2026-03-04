using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ScavengeMedicineOrderHandler : IOrderHandler
    {
        const double MedicineGain = 20;
        const double SicknessIncrease = 5;
        const double MedicineThreshold = 15;
        const int Deaths = 2;

        readonly ScavengeMedicineOrder _order;
        readonly IPopupService _popup;

        public ScavengeMedicineOrderHandler(ScavengeMedicineOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.Medicine < MedicineThreshold;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine += MedicineGain;
            log.Record("Medicine", MedicineGain, _order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, _order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, _order.Id);
            log.Record("Deaths", Deaths, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
