using System.Collections.Generic;
using AutofacUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Siege.Gameplay.Simulation
{
    /// <summary>
    /// Orchestrates the simulation tick pipeline. Ticks all registered systems each frame.
    /// Handles pause input (Space key) and day/night transitions.
    /// </summary>
    public class SimulationRunner : MonoBehaviour
    {
        readonly List<ISimulationSystem> _systems = new();

        GameState _state;
        GameClock _clock;
        ChangeLog _changeLog;
        bool _initialized;

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _changeLog = Resolver.Resolve<ChangeLog>();

            _state.Initialize();
            _clock.Initialize();

            _clock.DayStarted += OnDayStarted;
            _clock.NightStarted += OnNightStarted;
            _clock.DayEnded += OnDayEnded;

            // Fire day 1 start
            OnDayStarted(1);
            _initialized = true;
        }

        void OnDestroy()
        {
            if (_clock != null)
            {
                _clock.DayStarted -= OnDayStarted;
                _clock.NightStarted -= OnNightStarted;
                _clock.DayEnded -= OnDayEnded;
            }
        }

        public void RegisterSystem(ISimulationSystem system)
        {
            _systems.Add(system);
        }

        void Update()
        {
            if (!_initialized) return;

            HandlePauseInput();

            float dt = Time.deltaTime;
            _clock.Advance(dt);

            if (_clock.IsPaused) return;
            if (_state.IsGameOver) return;

            float scaledDt = dt * _clock.TimeScale;

            foreach (var system in _systems)
            {
                system.Tick(_state, scaledDt);
            }

            _state.ClampValues();
        }

        void HandlePauseInput()
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _clock.TogglePause();
            }
        }

        void OnDayStarted(int day)
        {
            _changeLog.FlushDay();
            _state.DeathsToday = 0;
            _state.EventsFiredToday = 0;

            foreach (var system in _systems)
                system.OnDayStart(_state, day);
        }

        void OnNightStarted(int day)
        {
            foreach (var system in _systems)
                system.OnNightStart(_state, day);
        }

        void OnDayEnded(int day)
        {
            // Update deficit tracking
            if (_state.Food <= 0)
                _state.ConsecutiveFoodDeficitDays++;
            else
                _state.ConsecutiveFoodDeficitDays = 0;

            if (_state.Water <= 0)
                _state.ConsecutiveWaterDeficitDays++;
            else
                _state.ConsecutiveWaterDeficitDays = 0;

            if (_state.Food <= 0 && _state.Water <= 0)
                _state.ConsecutiveBothDeficitDays++;
            else
                _state.ConsecutiveBothDeficitDays = 0;

            // Streak tracking
            if (_state.Food > 0 && _state.Water > 0)
                _state.ConsecutiveNoDeficitDays++;
            else
                _state.ConsecutiveNoDeficitDays = 0;

            if (_state.Sickness < 20)
                _state.ConsecutiveLowSicknessDays++;
            else
                _state.ConsecutiveLowSicknessDays = 0;

            // Decrement order cooldowns
            var keys = new List<string>(_state.OrderCooldowns.Keys);
            foreach (var key in keys)
            {
                _state.OrderCooldowns[key]--;
                if (_state.OrderCooldowns[key] <= 0)
                    _state.OrderCooldowns.Remove(key);
            }
        }
    }
}
