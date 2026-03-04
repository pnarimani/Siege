namespace Siege.Gameplay.Events
{
    public class DistantHornsEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "distant_horns";
        public string Name => "Distant Horns";
        public string Description => "Horns in the distance. Relief? Or the final assault? You cannot tell.";
        public int Priority => 90;
    }
}
