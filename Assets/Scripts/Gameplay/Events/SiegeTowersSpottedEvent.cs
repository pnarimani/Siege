namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "siege_towers_spotted";
        public string Name => "Siege Towers Spotted";
        public string Description => "Scouts report the enemy has built siege towers. The bombardment will intensify.";
        public int Priority => 90;
    }
}
