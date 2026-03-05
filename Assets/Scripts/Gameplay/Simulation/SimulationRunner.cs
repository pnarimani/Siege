using System;
using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Resources;

namespace Siege.Gameplay.Simulation
{
    /// <summary>
    /// Orchestrates the simulation tick pipeline. Ticks all registered systems each frame.
    /// Handles day/night transitions and end-of-day bookkeeping.
    /// </summary>
    public class SimulationRunner : ITickable, IDisposable
    {
        const double LowSicknessThreshold = 20;

        readonly List<ISimulationSystem> _systems = new();
        readonly List<string> _cooldownKeysToRemove = new();
        readonly GameState _state;
        readonly GameClock _clock;
        readonly ChangeLog _changeLog;
        readonly ResourceLedger _ledger;

        public SimulationRunner(GameState state, GameClock clock, ChangeLog changeLog, ResourceLedger ledger)
        {
            _state = state;
            _clock = clock;
            _changeLog = changeLog;
            _ledger = ledger;
        }

        public void Initialize()
        {
            _clock.Initialize();

            _clock.DayStarted += OnDayStarted;
            _clock.NightStarted += OnNightStarted;
            _clock.DayEnded += OnDayEnded;

            // Fire day 1 start
            OnDayStarted(1);
        }

        public void Dispose()
        {
            _clock.DayStarted -= OnDayStarted;
            _clock.NightStarted -= OnNightStarted;
            _clock.DayEnded -= OnDayEnded;
        }

        public void RegisterSystem(ISimulationSystem system)
        {
            _systems.Add(system);
        }

        public void Update(float deltaTime)
        {
            _clock.Advance(deltaTime);

            if (_clock.IsPaused) return;
            if (_state.IsGameOver) return;

            float scaledDt = deltaTime * _clock.TimeScale;

            for (int i = 0; i < _systems.Count; i++)
                _systems[i].Tick(_state, scaledDt);

            _state.ClampValues();
        }

        void OnDayStarted(int day)
        {
            _changeLog.FlushDay();
            _state.DeathsToday = 0;
            _state.EventsFiredToday = 0;
            _state.ActionUsedToday = false;

            for (int i = 0; i < _systems.Count; i++)
                _systems[i].OnDayStart(_state, day);
        }

        void OnNightStarted(int day)
        {
            _state.MissionLaunchedThisNight = false;

            for (int i = 0; i < _systems.Count; i++)
                _systems[i].OnNightStart(_state, day);
        }

        void OnDayEnded(int day)
        {
            // Update deficit tracking
            double food = _ledger.GetTotal(ResourceType.Food);
            double water = _ledger.GetTotal(ResourceType.Water);

            if (food <= 0)
                _state.ConsecutiveFoodDeficitDays++;
            else
                _state.ConsecutiveFoodDeficitDays = 0;

            if (water <= 0)
                _state.ConsecutiveWaterDeficitDays++;
            else
                _state.ConsecutiveWaterDeficitDays = 0;

            if (food <= 0 && water <= 0)
                _state.ConsecutiveBothDeficitDays++;
            else
                _state.ConsecutiveBothDeficitDays = 0;

            // Streak tracking
            if (food > 0 && water > 0)
                _state.ConsecutiveNoDeficitDays++;
            else
                _state.ConsecutiveNoDeficitDays = 0;

            if (_state.Sickness < LowSicknessThreshold)
                _state.ConsecutiveLowSicknessDays++;
            else
                _state.ConsecutiveLowSicknessDays = 0;

            // Tick down temporal modifiers
            if (_state.SiegeDamageReductionDays > 0)
            {
                _state.SiegeDamageReductionDays--;
                if (_state.SiegeDamageReductionDays <= 0)
                    _state.SiegeDamageReductionMultiplier = 1.0;
            }

            if (_state.TaintedWellDays > 0)
                _state.TaintedWellDays--;

            // Decrement order cooldowns (copy keys to avoid allocation from dict.Keys)
            _cooldownKeysToRemove.Clear();
            foreach (var kvp in _state.OrderCooldowns)
                _cooldownKeysToRemove.Add(kvp.Key);
            foreach (var key in _cooldownKeysToRemove)
            {
                _state.OrderCooldowns[key]--;
                if (_state.OrderCooldowns[key] <= 0)
                    _state.OrderCooldowns.Remove(key);
            }
        }
    }
}
