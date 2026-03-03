using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Siege
{
    /// <summary>
    /// Tracks relief army arrival and win condition.
    /// Arrival day is random (35-45), can be accelerated by player actions.
    /// </summary>
    public class ReliefArmy : ISimulationSystem
    {
        const int MinArrivalDay = 35;
        const int MaxArrivalDay = 45;
        const int SignalFireAcceleration = 3;

        readonly ChangeLog _changeLog;

        int _arrivalDay;
        bool _arrived;
        bool _initialized;

        public bool HasArrived => _arrived;
        public int ArrivalDay => _arrivalDay;

        public ReliefArmy(ChangeLog changeLog)
        {
            _changeLog = changeLog;
        }

        public void OnDayStart(GameState state, int day)
        {
            if (!_initialized)
            {
                _arrivalDay = UnityEngine.Random.Range(MinArrivalDay, MaxArrivalDay + 1);
                _initialized = true;
            }

            // Signal fire accelerates arrival
            if (state.SignalFireLit && _arrivalDay > day + 1)
            {
                _arrivalDay = System.Math.Max(day + 1, _arrivalDay - SignalFireAcceleration);
                state.SignalFireLit = false; // consume the effect
                _changeLog.Record("ReliefArmy", -SignalFireAcceleration, "Signal fire");
            }
        }

        public void Tick(GameState state, float deltaTime) { }

        /// <summary>
        /// Accelerate arrival by a number of days (from missions, events, etc.)
        /// </summary>
        public void Accelerate(int days)
        {
            _arrivalDay = System.Math.Max(1, _arrivalDay - days);
            _changeLog.Record("ReliefArmy", -days, "Player action");
        }

        /// <summary>
        /// Check if relief army should arrive on the given day.
        /// </summary>
        public bool ShouldArrive(int currentDay)
        {
            if (_arrived) return false;
            if (currentDay >= _arrivalDay)
            {
                _arrived = true;
                return true;
            }
            return false;
        }
    }
}
