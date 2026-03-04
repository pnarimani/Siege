namespace Siege.Gameplay.Orders
{
    public class SecretCorrespondenceOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "secret_correspondence";
        public string Name => "Secret Correspondence";
        public string Description => "Maintain covert communication with allies outside the walls. May yield supplies or hasten relief.";
        public string NarrativeText => "A bird lands on the tower at dawn, a coded message bound to its leg. Hope is a fragile thing.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
    }
}
