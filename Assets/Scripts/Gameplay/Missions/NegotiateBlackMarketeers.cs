namespace Siege.Gameplay.Missions
{
    public class NegotiateBlackMarketeers : IMission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "negotiate_black_marketeers";
        public string Name => "Negotiate with Black Marketeers";
        public string Description => "Deal with smugglers to secure scarce supplies. Risky.";
        public int DurationDays => 3;
        public int WorkerCost => 2;
    }
}
