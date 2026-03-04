namespace Siege.Gameplay.Events
{
    public class EnemySappersEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "enemy_sappers";
        public string Name => "Enemy Sappers";
        public string Description => "Enemy miners have been at work beneath the walls. The ground shudders as tunnels collapse, weakening every district.";
        public int Priority => 80;
    }
}
