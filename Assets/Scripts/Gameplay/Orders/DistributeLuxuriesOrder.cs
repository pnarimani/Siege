using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DistributeLuxuriesOrder : IOrder
    {
        const string Narrative = "Candles, blankets, a few carved toys. Small things that remind people they are still human.";
        const double FuelCost = 5;
        const double MaterialsCost = 5;
        const double MoraleGain = 10;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 3;
        const double MaterialsRequired = 20;
        const double FuelRequired = 20;

        readonly IPopupService _popup;

        public DistributeLuxuriesOrder(IPopupService popup)
        {
            _popup = popup;
        }

        public string Id => "distribute_luxuries";
        public string Name => "Distribute Luxuries";
        public string Description => "Hand out small comforts — fuel and trinkets — to ease suffering, though gatherings spread illness.";
        public int CooldownDays => 6;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsRequired && state.Fuel >= FuelRequired;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Fuel -= FuelCost;
            log.Record("Fuel", -FuelCost, Id);

            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new DistributeLuxuriesOrder(_popup);
    }
}
