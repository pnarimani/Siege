using System;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Population
{
    /// <summary>
    /// Handles population changes: sick workers dying, wounded guards dying,
    /// morale-based desertion, and elderly exhaustion if conscripted.
    /// </summary>
    public class PopulationSystem : ISimulationSystem
    {
        const int SickDeathThresholdDays = 3;
        const int WoundedDeathThresholdDays = 4;
        const double DesertionMoraleThreshold = 20;
        const double DesertionBaseChance = 0.05; // 5% per day when morale below threshold
        const double ElderlyExhaustionChance = 0.02; // 2% per day when conscripted

        readonly ChangeLog _changeLog;
        readonly WorkerAllocation _workerAllocation;

        // Tracked per day
        int _sickDayAccumulator;
        int _woundedDayAccumulator;
        bool _processedToday;

        public PopulationSystem(ChangeLog changeLog, WorkerAllocation workerAllocation)
        {
            _changeLog = changeLog;
            _workerAllocation = workerAllocation;
        }

        public void OnDayStart(GameState state, int day)
        {
            _processedToday = false;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_processedToday) return;
            _processedToday = true;

            ProcessSickDeaths(state);
            ProcessWoundedDeaths(state);
            ProcessDesertion(state);

            _workerAllocation.ValidateAssignments();
        }

        void ProcessSickDeaths(GameState state)
        {
            if (state.SickWorkers <= 0)
            {
                _sickDayAccumulator = 0;
                return;
            }

            _sickDayAccumulator++;

            if (_sickDayAccumulator >= SickDeathThresholdDays)
            {
                // Some sick workers die each day past the threshold
                int deaths = Math.Max(1, state.SickWorkers / 4);
                state.SickWorkers -= deaths;
                state.TotalDeaths += deaths;
                state.DeathsToday += deaths;
                _changeLog.Record("SickWorkers", -deaths, "Died from illness");
                _changeLog.Record("Deaths", deaths, "Illness");

                // Deaths increase unrest
                state.Unrest += deaths * 1.5;
                state.Morale -= deaths * 1.0;
                _changeLog.Record("Unrest", deaths * 1.5, "Deaths from illness");
                _changeLog.Record("Morale", -deaths * 1.0, "Deaths from illness");
            }
        }

        void ProcessWoundedDeaths(GameState state)
        {
            if (state.WoundedGuards <= 0)
            {
                _woundedDayAccumulator = 0;
                return;
            }

            _woundedDayAccumulator++;

            if (_woundedDayAccumulator >= WoundedDeathThresholdDays)
            {
                int deaths = Math.Max(1, state.WoundedGuards / 3);
                state.WoundedGuards -= deaths;
                state.TotalDeaths += deaths;
                state.DeathsToday += deaths;
                _changeLog.Record("WoundedGuards", -deaths, "Died from wounds");
                _changeLog.Record("Deaths", deaths, "Wounds");

                state.Morale -= deaths * 2.0;
                _changeLog.Record("Morale", -deaths * 2.0, "Guard deaths");
            }
        }

        void ProcessDesertion(GameState state)
        {
            if (state.Morale >= DesertionMoraleThreshold) return;

            double moraleDeficit = DesertionMoraleThreshold - state.Morale;
            double chance = DesertionBaseChance + (moraleDeficit * 0.005);

            if (UnityEngine.Random.value < chance)
            {
                int deserters = Math.Max(1, (int)(state.HealthyWorkers * 0.03));
                deserters = Math.Min(deserters, state.HealthyWorkers);

                if (deserters > 0)
                {
                    state.HealthyWorkers -= deserters;
                    _changeLog.Record("HealthyWorkers", -deserters, "Desertion");

                    state.Unrest += deserters * 0.5;
                    _changeLog.Record("Unrest", deserters * 0.5, "Desertion panic");
                }
            }
        }
    }
}
