using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Events
{
    [RegisterTypeLookup]
    public interface IGameEvent
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool CanTrigger(GameState state);
        void Execute(GameState state, ChangeLog log) { }
        void ExecuteResponse(GameState state, ChangeLog log, int responseIndex) { }
        EventResponse[] GetResponses(GameState state) => System.Array.Empty<EventResponse>();
        IGameEvent Clone();
    }
}
