namespace Siege.Gameplay.Orders
{
    public class ScavengeMedicineOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "scavenge_medicine";
        public string Name => "Scavenge Medicine";
        public string Description => "Send workers beyond the walls to scavenge for medicine. Some will not return.";
        public string NarrativeText => "Two volunteers slip through the breach at dawn. By nightfall, crates arrive. The volunteers do not.";
        public int CooldownDays => 3;
    }
}
