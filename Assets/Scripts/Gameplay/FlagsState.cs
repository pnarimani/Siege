using System.Collections.Generic;
using UnityEngine;

namespace Siege.Gameplay
{
    public class FlagsState
    {
        // ── Tyranny Path ──
        public IntFlag Tyranny { get; } = new(0, 0, 10);
        public BoolFlag IronFist { get; } = new();
        public BoolFlag MartialState { get; } = new();
        public BoolFlag MercyDenied { get; } = new();

        // ── Faith Path ──
        public IntFlag Faith { get; } = new(0, 0, 10);
        public BoolFlag FaithRisen { get; } = new();
        public BoolFlag PeopleFirst { get; } = new();

        // ── Fortification Path ──
        public IntFlag Fortification { get; } = new(0, 0, 10);
        public BoolFlag GarrisonState { get; } = new();
        public BoolFlag WallsHold { get; } = new();

        // ── Laws ──
        public BoolFlag CannibalismEnacted { get; } = new();

        // ── Shared ──
        public IntFlag FearLevel { get; } = new(0, 0, 5);

        // ── Humanity Score ──
        public IntFlag Humanity { get; } = new(50, 0, 100);

        public void TickDay()
        {
            Tyranny.TickDay();
            Faith.TickDay();
            Fortification.TickDay();
            FearLevel.TickDay();
            Humanity.TickDay();
        }
    }

    public sealed class IntFlag
    {
        readonly int _min;
        readonly int _max;
        readonly List<PendingExpiry> _expiries = new();

        public int Value { get; private set; }

        public IntFlag(int defaultValue, int min, int max)
        {
            _min = min;
            _max = max;
            Value = Mathf.Clamp(defaultValue, min, max);
        }

        public void Add(int delta, int? lifetimeDays = null)
        {
            Value = Mathf.Clamp(Value + delta, _min, _max);
            if (lifetimeDays.HasValue)
                _expiries.Add(new PendingExpiry(delta, lifetimeDays.Value));
        }

        public void TickDay()
        {
            for (var i = _expiries.Count - 1; i >= 0; i--)
            {
                _expiries[i].DaysRemaining--;
                if (_expiries[i].DaysRemaining <= 0)
                {
                    Value = Mathf.Clamp(Value - _expiries[i].Delta, _min, _max);
                    _expiries.RemoveAt(i);
                }
            }
        }

        public static implicit operator int(IntFlag flag) => flag.Value;

        sealed class PendingExpiry
        {
            public int Delta { get; }
            public int DaysRemaining { get; set; }

            public PendingExpiry(int delta, int daysRemaining)
            {
                Delta = delta;
                DaysRemaining = daysRemaining;
            }
        }
    }

    public sealed class BoolFlag
    {
        public bool Value { get; private set; }

        public void Set(bool value = true) => Value = value;

        public static implicit operator bool(BoolFlag flag) => flag.Value;
    }
}