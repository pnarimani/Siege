namespace Siege.Gameplay.Missions
{
    public class EngineerTunnels : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "engineer_tunnels";
        public string Name => "Engineer Tunnels";
        public string Description => "Dig counter-tunnels to undermine enemy siege works.";
        public int DurationDays => 4;
        public int WorkerCost => 5;
    }
}
