namespace Siege.Gameplay.Missions
{
    public class RaidCivilianFarms : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "raid_civilian_farms";
        public string Name => "Raid Civilian Farms";
        public string Description => "Take food from farms outside the walls. The farmers won't be happy.";
        public int DurationDays => 2;
        public int WorkerCost => 4;
    }
}
