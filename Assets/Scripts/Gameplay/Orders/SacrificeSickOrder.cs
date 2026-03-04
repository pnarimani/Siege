namespace Siege.Gameplay.Orders
{
    public class SacrificeSickOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "sacrifice_sick";
        public string Name => "Sacrifice the Sick";
        public string Description => "Remove the terminally ill to slow the spread of disease. A decision no one will forget.";
        public string NarrativeText => "They are taken beyond the walls at night. No one speaks of it in the morning.";
        public int CooldownDays => 3;
    }
}
