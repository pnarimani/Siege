using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeEngineersArriveEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "siege_engineers_arrive";
        public string Name => "Siege Engineers Arrive";
        public string Description => "A small band of military engineers approaches the gate, offering their skills in exchange for shelter and rations.";
        public bool IsOneTime => true;
        public int Priority => 50;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Accept them",
                    "+3 Workers, +20 Materials, -10 Food, +1 Fortification"),
                new EventResponse(
                    "Decline",
                    "+5 Morale")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
