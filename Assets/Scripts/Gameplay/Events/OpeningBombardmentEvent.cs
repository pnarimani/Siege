namespace Siege.Gameplay.Events
{
    public class OpeningBombardmentEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "opening_bombardment";
        public string Name => "Opening Bombardment";
        public string Description => "The first boulders crash into Outer Farms. Smoke rises from burning granaries as the enemy demonstrates their intent.";
        public int Priority => 100;
    }
}
