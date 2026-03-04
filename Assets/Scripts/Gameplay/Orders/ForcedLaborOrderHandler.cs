using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ForcedLaborOrderHandler : OrderHandler<ForcedLaborOrder>
    {
        const double MaterialsGain = 15;
        const double UnrestIncrease = 8;
        const int Deaths = 2;

        readonly PoliticalState _political;

        public ForcedLaborOrderHandler(ForcedLaborOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            _political.Faith.Value < 4;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, Order.Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Order.Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Order.Id);
            log.Record("Deaths", Deaths, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
