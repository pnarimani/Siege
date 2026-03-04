namespace Siege.Gameplay.Orders
{
    public class PublicTrialOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "public_trial";
        public string Name => "Public Trial";
        public string Description => "Hold a public trial to make an example of dissenters.";
        public string NarrativeText => "The verdict was decided before the trial began. Everyone knows it. No one objects.";
        public int CooldownDays => 5;
    }
}
