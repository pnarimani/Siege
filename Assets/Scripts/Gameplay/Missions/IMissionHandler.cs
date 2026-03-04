using JetBrains.Annotations;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    [UsedImplicitly]
    public interface IMissionHandler
    {
        string MissionId { get; }
        bool CanLaunch(GameState state);
        MissionOutcome Resolve(GameState state, ChangeLog log);
    }
}
