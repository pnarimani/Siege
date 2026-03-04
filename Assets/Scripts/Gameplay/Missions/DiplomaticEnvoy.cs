namespace Siege.Gameplay.Missions
{
    public class DiplomaticEnvoy : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "diplomatic_envoy";
        public string Name => "Diplomatic Envoy";
        public string Description => "Send envoys to seek terms or stall the enemy.";
        public int DurationDays => 3;
        public int WorkerCost => 3;
    }
}
