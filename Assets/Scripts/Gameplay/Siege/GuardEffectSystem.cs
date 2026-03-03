using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Siege
{
    /// <summary>
    /// Guards reduce unrest passively and contribute to siege damage reduction.
    /// This system ticks the passive unrest reduction.
    /// </summary>
    public class GuardEffectSystem : ISimulationSystem
    {
        const double UnrestReductionPerGuard = 0.3; // per guard per day
        const double MaxDailyUnrestReduction = 5.0;

        readonly ChangeLog _changeLog;

        public GuardEffectSystem(ChangeLog changeLog)
        {
            _changeLog = changeLog;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (state.Guards <= 0) return;

            float dayFraction = deltaTime / GameClock.DayLengthSeconds;
            double reduction = System.Math.Min(
                state.Guards * UnrestReductionPerGuard * dayFraction,
                MaxDailyUnrestReduction * dayFraction);

            if (reduction > 0 && state.Unrest > 0)
            {
                state.Unrest -= reduction;
                _changeLog.Record("Unrest", -reduction, "Guard patrol");
            }
        }
    }
}
