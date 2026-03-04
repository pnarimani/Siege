namespace Siege.Gameplay.Missions
{
    public class ScoutingMission : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "scouting_mission";
        public string Name => "Scouting Mission";
        public string Description => "Send scouts to gather intelligence on enemy positions.";
        public int DurationDays => 2;
        public int WorkerCost => 2;
    }
}
