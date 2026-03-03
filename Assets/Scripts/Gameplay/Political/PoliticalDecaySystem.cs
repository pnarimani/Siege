using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Political
{
    /// <summary>
    /// Applies daily decay to all political tracks that have decay configured.
    /// </summary>
    public class PoliticalDecaySystem : ISimulationSystem
    {
        readonly PoliticalState _political;

        public PoliticalDecaySystem(PoliticalState political)
        {
            _political = political;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            _political.ApplyDecay();
        }
    }
}
