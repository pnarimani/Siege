using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeBombardmentEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "siege_bombardment";
        public string Name => "Siege Bombardment";
        public string Description => "The active perimeter sustains damage from bombardment. Stone and flame rain down without warning.";
        public bool IsOneTime => false;
        public int Priority => 50;

        public string GetNarrativeText(GameState state) => Description;
    }
}
