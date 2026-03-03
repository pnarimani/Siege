using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Resources
{
    /// <summary>
    /// Care is produced by Clinics and consumed immediately to heal sick workers and wounded guards.
    /// One care unit heals one person. Prioritizes wounded guards, then sick workers.
    /// </summary>
    public class CareSystem : ISimulationSystem
    {
        readonly ChangeLog _changeLog;

        public CareSystem(ChangeLog changeLog)
        {
            _changeLog = changeLog;
        }

        public void OnDayStart(GameState state, int day)
        {
            // Care is accumulated through the day via production, then applied at day start
            ApplyCare(state);
        }

        public void Tick(GameState state, float deltaTime)
        {
            // Care accumulates during the tick via ResourceProductionSystem.
            // We apply it continuously so healing feels responsive.
            ApplyCare(state);
        }

        void ApplyCare(GameState state)
        {
            double care = state.GetResource(ResourceType.Care);
            if (care < 1.0) return;

            int careUnits = (int)care;

            // Heal wounded guards first (they're more urgent for defense)
            int woundedHealed = Math.Min(careUnits, state.WoundedGuards);
            if (woundedHealed > 0)
            {
                state.WoundedGuards -= woundedHealed;
                state.Guards += woundedHealed;
                careUnits -= woundedHealed;
                _changeLog.Record("WoundedGuards", -woundedHealed, "Clinic care");
                _changeLog.Record("Guards", woundedHealed, "Clinic care");
            }

            // Then heal sick workers
            int sickHealed = Math.Min(careUnits, state.SickWorkers);
            if (sickHealed > 0)
            {
                state.SickWorkers -= sickHealed;
                state.HealthyWorkers += sickHealed;
                careUnits -= sickHealed;
                _changeLog.Record("SickWorkers", -sickHealed, "Clinic care");
                _changeLog.Record("HealthyWorkers", sickHealed, "Clinic care");
            }

            // Care is consumed; set remaining (fractional) amount
            state.SetResource(ResourceType.Care, care - (int)care);
        }
    }
}
