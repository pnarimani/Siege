namespace Siege.Gameplay.Simulation
{
    public readonly struct StateChange
    {
        public readonly string Field;
        public readonly double Amount;
        public readonly string Source;

        public StateChange(string field, double amount, string source)
        {
            Field = field;
            Amount = amount;
            Source = source;
        }

        public override string ToString() =>
            $"{Field}: {(Amount >= 0 ? "+" : "")}{Amount:F1} ({Source})";
    }
}
