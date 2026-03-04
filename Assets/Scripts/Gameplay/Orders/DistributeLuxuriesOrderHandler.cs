using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DistributeLuxuriesOrderHandler : OrderHandler<DistributeLuxuriesOrder>
    {
        const double FuelCost = 5;
        const double MaterialsCost = 5;
        const double MoraleGain = 10;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 3;
        const double MaterialsRequired = 20;
        const double FuelRequired = 20;

        public DistributeLuxuriesOrderHandler(DistributeLuxuriesOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsRequired && state.Fuel >= FuelRequired;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Order.Id);

            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Order.Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Order.Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Order.Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
