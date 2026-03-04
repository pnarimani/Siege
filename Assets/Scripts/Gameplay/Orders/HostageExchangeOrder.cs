namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "hostage_exchange";
        public string Name => "Hostage Exchange";
        public string Description => "Trade supplies to recover captured citizens. A slow, costly process.";
        public string NarrativeText => "A figure stumbles through the gate, gaunt and shaking. One more mouth to feed. One more soul saved.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
    }
}
