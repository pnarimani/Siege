using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FeverOutbreakEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "fever_outbreak";
        public string Name => "Fever Outbreak";
        public string Description => "The fever clinic overflows. Bodies are carried out faster than the living can be carried in.";
        public bool IsOneTime => true;
        public int Priority => 70;

        public string GetNarrativeText(GameState state) => Description;
    }
}
