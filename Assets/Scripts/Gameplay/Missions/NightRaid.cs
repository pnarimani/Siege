namespace Siege.Gameplay.Missions
{
    public class NightRaid : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "night_raid";
        public string Name => "Night Raid";
        public string Description => "Launch a daring night raid against enemy siege works.";
        public int DurationDays => 2;
        public int WorkerCost => 6;
    }
}
