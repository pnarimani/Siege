using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    public class MissionProgressSystem : ISimulationSystem
    {
        readonly MissionManager _missionManager;

        public MissionProgressSystem(MissionManager missionManager)
        {
            _missionManager = missionManager;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            _missionManager.AdvanceDay(state);
        }
    }
}
