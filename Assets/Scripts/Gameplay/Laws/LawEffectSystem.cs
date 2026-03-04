using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    /// <summary>
    /// Ticks all enacted laws each day for their ongoing effects.
    /// </summary>
    public class LawEffectSystem : ISimulationSystem
    {
        readonly LawDispatcher _lawDispatcher;
        bool _processedToday;

        public LawEffectSystem(LawDispatcher lawDispatcher)
        {
            _lawDispatcher = lawDispatcher;
        }

        public void OnDayStart(GameState state, int day)
        {
            _processedToday = false;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_processedToday) return;
            _processedToday = true;
            _lawDispatcher.TickAll();
        }
    }
}
