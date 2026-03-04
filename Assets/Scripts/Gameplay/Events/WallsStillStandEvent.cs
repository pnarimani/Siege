using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallsStillStandEvent : GameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "walls_still_stand";
        public string Name => "The Walls Still Stand";
        public string Description => "The garrison's resolve inspires the city.";
        public int Priority => 20;
        public bool IsOneTime => false;

        public string GetNarrativeText(GameState state) =>
            "The walls hold firm. The garrison's determination inspires the whole city.";
    }
}
