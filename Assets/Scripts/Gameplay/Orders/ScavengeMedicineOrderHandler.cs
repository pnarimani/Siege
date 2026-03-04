using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ScavengeMedicineOrderHandler : OrderHandler<ScavengeMedicineOrder>
    {
        const double MedicineGain = 20;
        const double SicknessIncrease = 5;
        const double MedicineThreshold = 15;
        const int Deaths = 2;

        public ScavengeMedicineOrderHandler(ScavengeMedicineOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Medicine < MedicineThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine += MedicineGain;
            log.Record("Medicine", MedicineGain, Order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Order.Id);
            log.Record("Deaths", Deaths, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
