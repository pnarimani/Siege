using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    /// <summary>
    /// Ticks all enacted laws each day for their ongoing effects.
    /// </summary>
    public class LawEffectSystem : ISimulationSystem
    {
        readonly LawManager _lawManager;
        readonly ChangeLog _changeLog;
        bool _processedToday;

        public LawEffectSystem(LawManager lawManager, ChangeLog changeLog)
        {
            _lawManager = lawManager;
            _changeLog = changeLog;
        }

        public void OnDayStart(GameState state, int day)
        {
            _processedToday = false;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_processedToday) return;
            _processedToday = true;

            foreach (var law in _lawManager.AllLaws)
            {
                if (law.IsEnacted)
                    law.OnDayTick(state, _changeLog);
            }
        }
    }
}
