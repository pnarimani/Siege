namespace Siege.Gameplay.Events
{
    public class BurningFarmsEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "burning_farms";
        public string Name => "Burning Farms";
        public string Description => "Smoke rises beyond the walls. The enemy is burning the farms that once fed this city. There will be no harvest.";
        public int Priority => 90;
    }
}
