namespace Siege.Gameplay.Orders
{
    public class FortifyGateOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "fortify_gate";
        public string Name => "Fortify Gate";
        public string Description => "Reinforce the gate when defenses are weakened.";
        public string NarrativeText => "Iron and timber are hammered into the gate. It groans but holds.";
        public int CooldownDays => 3;
    }
}
