namespace Siege.Gameplay.Missions
{
    public class SabotageEnemySupplies : IMission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "sabotage_enemy_supplies";
        public string Name => "Sabotage Enemy Supplies";
        public string Description => "Infiltrate enemy camps and destroy their supply lines.";
        public int DurationDays => 3;
        public int WorkerCost => 4;
    }
}
