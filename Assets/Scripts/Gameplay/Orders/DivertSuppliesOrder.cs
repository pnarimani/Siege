namespace Siege.Gameplay.Orders
{
    public class DivertSuppliesOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "divert_supplies";
        public string Name => "Divert Supplies";
        public string Description => "Redirect materials and fuel to boost repair output for today.";
        public string NarrativeText => "Every nail, every plank is accounted for. Today, the builders get everything they need.";
        public int CooldownDays => 3;
    }
}
