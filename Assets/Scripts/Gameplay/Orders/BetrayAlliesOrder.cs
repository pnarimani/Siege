namespace Siege.Gameplay.Orders
{
    public class BetrayAlliesOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "betray_allies";
        public string Name => "Betray Allies";
        public string Description => "Sell out allied contacts for a windfall of supplies. Permanent consequences and daily retaliation risk.";
        public string NarrativeText => "The deal is struck in a ruined chapel. Thirty crates for thirty names. There is no going back.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
        public bool CanDeactivate => false;
    }
}
