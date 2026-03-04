namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "siege_towers_spotted";
        public string Name => "Siege Towers Spotted";
        public string Description => "Scouts report the enemy has built siege towers. The bombardment will intensify.";
        public int Priority => 90;
    }
}
