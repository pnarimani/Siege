namespace Siege.Gameplay.Missions
{
    public class Sortie : IMission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "sortie";
        public string Name => "Sortie";
        public string Description => "Lead guards in a direct assault on enemy positions.";
        public int DurationDays => 1;
        public int WorkerCost => 0;
        public int GuardCost => 8;
    }
}
