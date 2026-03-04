using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Population
{
    /// <summary>
    /// Sickness progression: workers become sick from various sources,
    /// sickness status affects overall city health. Without care, sick workers die.
    /// </summary>
    public class SicknessSystem : ISimulationSystem
    {
        const double BaseSicknessRate = 0.5; // base sickness gain per day
        const double NoMedicinePenalty = 2.0;
        const double PlagueRatsPenalty = 3.0;
        const double TaintedWellPenalty = 1.5;
        const double SicknessToWorkerThreshold = 30; // sickness % above which workers become sick
        const double WorkerSicknessRate = 0.03; // 3% of healthy workers get sick per day above threshold

        readonly ChangeLog _changeLog;
        bool _processedToday;

        public SicknessSystem(ChangeLog changeLog)
        {
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

            ProcessDailySickness(state);
        }

        void ProcessDailySickness(GameState state)
        {
            double sicknessGain = BaseSicknessRate;

            // No medicine amplifies sickness
            if (state.Medicine <= 0)
                sicknessGain += NoMedicinePenalty;

            // Active plague events
            if (state.PlagueRatsActive)
                sicknessGain += PlagueRatsPenalty;

            // Tainted well (countdown managed by SimulationRunner.OnDayEnded)
            if (state.TaintedWellDays > 0)
            {
                sicknessGain += TaintedWellPenalty;
            }

            state.Sickness += sicknessGain;
            _changeLog.Record("Sickness", sicknessGain, "Daily sickness");

            // High sickness makes workers sick
            if (state.Sickness > SicknessToWorkerThreshold && state.HealthyWorkers > 0)
            {
                double excessSickness = state.Sickness - SicknessToWorkerThreshold;
                int newSick = (int)(state.HealthyWorkers * WorkerSicknessRate * (excessSickness / 100.0));
                newSick = System.Math.Max(0, System.Math.Min(newSick, state.HealthyWorkers));

                if (newSick > 0)
                {
                    state.HealthyWorkers -= newSick;
                    state.SickWorkers += newSick;
                    _changeLog.Record("HealthyWorkers", -newSick, "Fell ill");
                    _changeLog.Record("SickWorkers", newSick, "New illness");
                }
            }

            // Natural sickness recovery when sickness is low
            if (state.Sickness > 0 && state.Medicine > 0)
            {
                double recovery = System.Math.Min(1.0, state.Medicine * 0.1);
                state.Sickness -= recovery;
                _changeLog.Record("Sickness", -recovery, "Natural recovery");
            }
        }
    }
}
