using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    public class MissionProgressSystem : ISimulationSystem
    {
        readonly MissionDispatcher _missionDispatcher;

        public MissionProgressSystem(MissionDispatcher missionDispatcher)
        {
            _missionDispatcher = missionDispatcher;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            _missionDispatcher.AdvanceDay(state);
        }
    }
}
