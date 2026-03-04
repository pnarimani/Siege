using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BetrayalFromWithinEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "betrayal_from_within";
        public string Name => "Betrayal from Within";
        public string Description => "A faction of guards has been meeting in secret. They plan to open the gates at dawn. You learn of it just in time.";
        public bool IsOneTime => true;
        public int Priority => 80;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Offer amnesty",
                    "Defectors rejoin as workers. +5 Morale."),
                new EventResponse(
                    "Make an example",
                    "Execute the ringleaders. The rest become workers. +10 Unrest."),
                new EventResponse(
                    "Let them go",
                    "Lose the defectors entirely. Risk unrest if garrison thins.")
            };
        }
        public string GetNarrativeText(GameState state) => Description;
    }
}
