using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DissidentsDiscoveredEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "dissidents_discovered";
        public string Name => "Dissidents Discovered";
        public string Description => "A ring of dissidents is uncovered plotting against the council. The city watches to see what you will do.";
        public bool IsOneTime => true;
        public int Priority => 55;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Execute them",
                    "-15 Unrest, +3 Deaths, -5 Morale, +1 Tyranny, +1 Fear"),
                new EventResponse(
                    "Imprison them",
                    "-10 Unrest, +5 Morale"),
                new EventResponse(
                    "Release them",
                    "+5 Morale, +8 Unrest, +1 Faith")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
