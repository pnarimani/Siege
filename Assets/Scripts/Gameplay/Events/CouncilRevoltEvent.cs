using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CouncilRevoltEvent : GameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "council_revolt";
        public string Name => "Council Revolt";
        public string Description => "The council has seized control.";
        public int Priority => 200;

        public string GetNarrativeText(GameState state) =>
            "Your reign ends in bloodshed. The council has taken over.";
    }
}
