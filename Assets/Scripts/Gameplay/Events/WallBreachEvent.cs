using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallBreachEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "wall_breach";
        public string Name => "Wall Breach";
        public string Description => "The perimeter wall groans and splits. The enemy will not wait long to exploit the gap.";
        public bool IsOneTime => false;
        public int Priority => 85;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Reinforce the breach",
                    state.Guards >= 15
                        ? "Guards hold the line — breach contained."
                        : "Not enough guards — the wall buckles further."),
                new EventResponse(
                    "Barricade with materials",
                    "-10 Materials, minor integrity loss."),
                new EventResponse(
                    "Fall back",
                    "Abandon the section — significant integrity loss.")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
