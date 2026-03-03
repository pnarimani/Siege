using System;
using UnityEngine;

namespace Siege.Gameplay.Simulation
{
    /// <summary>
    /// Manages real-time day progression, day/night phases, and pause.
    /// One in-game day = DayLengthSeconds real seconds.
    /// Day phase = first DayPhaseFraction of the day, Night = remainder.
    /// </summary>
    public class GameClock
    {
        // ── Configuration ─────────────────────────────────────────────
        public const float DayLengthSeconds = 60f;
        public const float DayPhaseFraction = 0.70f; // 70% day, 30% night

        // ── State ─────────────────────────────────────────────────────
        public int CurrentDay { get; private set; } = 1;
        public float DayProgress { get; private set; } // 0..1 within current day
        public bool IsPaused { get; set; }
        public float TimeScale { get; set; } = 1f;

        public bool IsDay => DayProgress < DayPhaseFraction;
        public bool IsNight => !IsDay;

        public float DayPhaseProgress => IsDay
            ? DayProgress / DayPhaseFraction
            : 0f;

        public float NightPhaseProgress => IsNight
            ? (DayProgress - DayPhaseFraction) / (1f - DayPhaseFraction)
            : 0f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<int> DayStarted;    // fires at start of each day
        public event Action<int> NightStarted;  // fires when night phase begins
        public event Action<int> DayEnded;      // fires at end of each day (before next DayStarted)

        bool _nightStartedThisDay;

        public void Initialize()
        {
            CurrentDay = 1;
            DayProgress = 0f;
            _nightStartedThisDay = false;
            IsPaused = false;
            TimeScale = 1f;
        }

        /// <summary>
        /// Advance the clock by deltaTime. Returns true if a new day started.
        /// </summary>
        public bool Advance(float deltaTime)
        {
            if (IsPaused) return false;

            float scaledDelta = deltaTime * TimeScale;
            float progressDelta = scaledDelta / DayLengthSeconds;
            DayProgress += progressDelta;

            // Check night transition
            if (!_nightStartedThisDay && DayProgress >= DayPhaseFraction)
            {
                _nightStartedThisDay = true;
                NightStarted?.Invoke(CurrentDay);
            }

            // Check day rollover
            if (DayProgress >= 1f)
            {
                DayProgress -= 1f;
                DayEnded?.Invoke(CurrentDay);
                CurrentDay++;
                _nightStartedThisDay = false;
                DayStarted?.Invoke(CurrentDay);
                return true;
            }

            return false;
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }
    }
}
