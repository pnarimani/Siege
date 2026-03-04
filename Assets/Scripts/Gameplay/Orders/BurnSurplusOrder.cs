namespace Siege.Gameplay.Orders
{
    public class BurnSurplusOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "burn_surplus";
        public string Name => "Burn Surplus";
        public string Description => "Burn contaminated materials to cleanse the area, reducing sickness and lifting spirits.";
        public string NarrativeText => "The pyre burns high. The stench of rot gives way to clean smoke. People breathe a little easier.";
        public int CooldownDays => 3;
    }
}
