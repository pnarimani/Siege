using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CrisisOfFaithEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "crisis_of_faith";
        public string Name => "Crisis of Faith";
        public string Description => "The faithful gather in the square, torn between devotion and despair. They look to you for guidance.";
        public bool IsOneTime => true;
        public int Priority => 60;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Hold a vigil",
                    "+20 Morale, -10 Food, +5 Sickness, +1 Faith"),
                new EventResponse(
                    "Abandon the faith",
                    "-5 Morale, +10 Unrest, -3 Faith")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
