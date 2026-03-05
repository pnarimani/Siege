using Siege.Gameplay.Resources;
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
        readonly ResourceLedger _ledger;

        public DistributeLuxuriesOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "distribute_luxuries";
        public string Name => "Distribute Luxuries";
        public string Description => "Hand out small comforts — fuel and trinkets — to ease suffering, though gatherings spread illness.";
        public int CooldownDays => 6;

        public bool CanIssue(GameState state) =>
            _ledger.Has(ResourceType.Materials, MaterialsRequired) && _ledger.Has(ResourceType.Fuel, FuelRequired);

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Withdraw(ResourceType.Fuel, FuelCost);
            log.Record("Fuel", -FuelCost, Id);

            _ledger.Withdraw(ResourceType.Materials, MaterialsCost);
            log.Record("Materials", -MaterialsCost, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new DistributeLuxuriesOrder(_popup, _ledger);
    }
}
