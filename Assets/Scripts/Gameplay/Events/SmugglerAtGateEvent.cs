using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SmugglerAtGateEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "smuggler_at_gate";
        public string Name => "Smuggler at the Gate";
        public string Description => "A smuggler offers goods at the gate. His prices are steep, but the city is hungry.";
        public int Priority => 60;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept the deal", "Food +20, Materials -15"),
            new EventResponse("Demand a better deal", "Food +30, Materials -15, Unrest +5"),
            new EventResponse("Turn him away", "No trade")
        };

        public string GetNarrativeText(GameState state) =>
            "A cloaked figure at the postern gate. He has food—but wants materials in return. Do you deal?";
    }
}
