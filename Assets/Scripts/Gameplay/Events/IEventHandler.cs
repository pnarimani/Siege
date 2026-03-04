using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Events
{
    [RegisterTypeLookup]
    public interface IEventHandler
    {
        string EventId { get; }
        bool CanTrigger(GameState state);
        void Execute(GameState state, ChangeLog log) { }
        void ExecuteResponse(GameState state, ChangeLog log, int responseIndex) { }
    }
}