using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DistributeLuxuriesOrder : Order
    {
        const int Cooldown = 6;
        const double FuelCost = 5;
        const double MaterialsCost = 5;
        const double MoraleGain = 10;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 3;
        const double MaterialsRequired = 20;
        const double FuelRequired = 20;

        public override string Id => "distribute_luxuries";
        public override string Name => "Distribute Luxuries";
        public override string Description => "Hand out small comforts — fuel and trinkets — to ease suffering, though gatherings spread illness.";
        public override string NarrativeText => "Candles, blankets, a few carved toys. Small things that remind people they are still human.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsRequired && state.Fuel >= FuelRequired;

        public override void Execute(GameState state, ChangeLog log)
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
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
