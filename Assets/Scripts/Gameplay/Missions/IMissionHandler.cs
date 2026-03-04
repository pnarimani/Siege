using JetBrains.Annotations;
using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Missions
{
    [UsedImplicitly]
    [RegisterTypeLookup]
    public interface IMissionHandler
    {
        string MissionId { get; }
        bool CanLaunch(GameState state);
        MissionOutcome Resolve(GameState state, ChangeLog log);
    }
}
