using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public interface GameEvent
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool HasTriggered { get; set; }
        bool IsOneTime => true;
        int Priority => 0;
        bool IsRespondable => false;
        EventResponse[] GetResponses(GameState state) => System.Array.Empty<EventResponse>();
        string GetNarrativeText(GameState state) => Description;
    }
}
