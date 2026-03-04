using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public interface IEventHandler
    {
        string EventId { get; }
        bool CanTrigger(GameState state);
        void Execute(GameState state, ChangeLog log);
        void ExecuteResponse(GameState state, ChangeLog log, int responseIndex);
    }
}
