namespace Siege.Gameplay.Events
{
    public class TaintedWellEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "tainted_well";
        public string Name => "Tainted Well";
        public string Description => "The well-keeper reports a foul smell from the main cistern. By the time the warning spreads, many have already drunk.";
        public int Priority => 70;
    }
}
