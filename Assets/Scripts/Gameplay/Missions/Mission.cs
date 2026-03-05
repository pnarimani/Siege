using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Missions
{
    [RegisterTypeLookup]
    public interface IMission
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool CanLaunch(GameState state);
        void OnLaunch(GameState state, ChangeLog log);
        void AdvanceDay(GameState state, ChangeLog log) { }
        bool IsComplete { get; }
        MissionOutcome Complete(GameState state, ChangeLog log);
        void OnCancelled(GameState state, ChangeLog log) { }
        float Progress { get; }
        IMission Clone();
    }
}
