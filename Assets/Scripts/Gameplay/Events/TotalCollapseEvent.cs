using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TotalCollapseEvent : IGameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "total_collapse";
        public string Name => "Total Collapse";
        public string Description => "All supplies are exhausted. The city collapses.";
        public int Priority => 200;

        public string GetNarrativeText(GameState state) =>
            "The last rations are gone. The last barrels are dry. " +
            "People collapse in the streets.";
    }
}
