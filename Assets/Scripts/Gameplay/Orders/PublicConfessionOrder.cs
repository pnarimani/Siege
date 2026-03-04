namespace Siege.Gameplay.Orders
{
    public class PublicConfessionOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "public_confession";
        public string Name => "Public Confession";
        public string Description => "Force accused dissidents to confess publicly, crushing unrest through fear.";
        public string NarrativeText => "They kneel in the square and recite their crimes. Whether the words are true no longer matters.";
        public int CooldownDays => 3;
    }
}
