using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HealthImprovingEvent : IGameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "health_improving";
        public string Name => "Health Improving";
        public string Description => "Days of low sickness restore confidence.";
        public int Priority => 20;
        public bool IsOneTime => false;

        public string GetNarrativeText(GameState state) =>
            "Days of low sickness have restored confidence.";
    }
}
