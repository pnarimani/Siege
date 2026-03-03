using System;

namespace Siege.Gameplay.Political
{
    /// <summary>
    /// A single political track (Tyranny, Faith, etc.) with value, optional bounds, and optional decay.
    /// </summary>
    [Serializable]
    public class PoliticalTrack
    {
        public readonly string Name;
        public int Value;
        public readonly int Min;
        public readonly int Max;
        public readonly float DecayPerDay; // 0 = no decay, positive = decays toward 0

        public PoliticalTrack(string name, int startValue = 0, int min = 0, int max = 20, float decayPerDay = 0f)
        {
            Name = name;
            Value = startValue;
            Min = min;
            Max = max;
            DecayPerDay = decayPerDay;
        }

        public void Add(int amount)
        {
            Value = Math.Clamp(Value + amount, Min, Max);
        }

        public void ApplyDecay()
        {
            if (DecayPerDay <= 0 || Value == 0) return;

            if (Value > 0)
                Value = Math.Max(0, Value - (int)Math.Ceiling(DecayPerDay));
            else
                Value = Math.Min(0, Value + (int)Math.Ceiling(DecayPerDay));
        }

        public static bool operator >=(PoliticalTrack track, int value) => track.Value >= value;
        public static bool operator <=(PoliticalTrack track, int value) => track.Value <= value;
        public static bool operator >(PoliticalTrack track, int value) => track.Value > value;
        public static bool operator <(PoliticalTrack track, int value) => track.Value < value;

        public override string ToString() => $"{Name}: {Value}";
    }
}
