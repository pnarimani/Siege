namespace Siege.Gameplay.Events
{
    public class EnemyCommanderLetterEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "enemy_commander_letter";
        public string Name => "Enemy Commander's Letter";
        public string Description => "A letter from the enemy commander: 'Your walls weaken. Your people starve. How long will you sacrifice them for pride?'";
        public int Priority => 90;
    }
}
