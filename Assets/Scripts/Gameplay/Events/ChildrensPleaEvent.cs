using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ChildrensPleaEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "childrens_plea";
        public string Name => "Children's Plea";
        public string Description => "A group of orphaned children approaches the council, begging for shelter and food.";
        public bool IsOneTime => true;
        public int Priority => 50;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Grant them shelter",
                    "-10 Materials, +10 Morale, +3 Sickness, +1 Faith"),
                new EventResponse(
                    "Refuse them",
                    "-5 Morale, +5 Unrest, +1 Tyranny")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
