namespace Siege.Gameplay.Missions
{
    public class ForageBeyondWalls : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "forage_beyond_walls";
        public string Name => "Forage Beyond Walls";
        public string Description => "Send workers to forage for food outside the walls.";
        public int DurationDays => 4;
        public int WorkerCost => 5;
    }
}
