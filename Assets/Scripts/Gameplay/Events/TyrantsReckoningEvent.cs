using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TyrantsReckoningEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "tyrants_reckoning";
        public string Name => "The Tyrant's Reckoning";
        public string Description => "The people have had enough. A delegation arrives at the council chamber — not asking, but demanding change.";
        public bool IsOneTime => true;
        public int Priority => 75;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Double down",
                    "+1 Tyranny, -30 Morale. Crush all dissent."),
                new EventResponse(
                    "Show mercy",
                    "-20 Unrest, +15 Morale, +2 Faith, -3 Tyranny. A rare olive branch.")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
