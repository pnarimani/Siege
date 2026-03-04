namespace Siege.Gameplay.Orders
{
    public class DistributeLuxuriesOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "distribute_luxuries";
        public string Name => "Distribute Luxuries";
        public string Description => "Hand out small comforts — fuel and trinkets — to ease suffering, though gatherings spread illness.";
        public string NarrativeText => "Candles, blankets, a few carved toys. Small things that remind people they are still human.";
        public int CooldownDays => 6;
    }
}
