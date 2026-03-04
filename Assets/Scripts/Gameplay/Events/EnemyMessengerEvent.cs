namespace Siege.Gameplay.Events
{
    public class EnemyMessengerEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "enemy_messenger";
        public string Name => "Enemy Messenger";
        public string Description => "A messenger arrives under white flag. 'Surrender the city, and your people will be spared.' You send him back.";
        public int Priority => 90;
    }
}
