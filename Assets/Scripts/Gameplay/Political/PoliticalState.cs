namespace Siege.Gameplay.Political
{
    /// <summary>
    /// Holds all political tracks and governance flags.
    /// Tracks accumulate from player actions (laws, orders, missions, events).
    /// Flags are derived from track thresholds or set by specific actions.
    /// </summary>
    public class PoliticalState
    {
        // ── Track Threshold Constants ─────────────────────────────────
        const int IronFistThreshold = 6;
        const int PeopleFirstFaithThreshold = 5;
        const int PeopleFirstTyrannyMax = 2;
        const int MartialStateTyrannyThreshold = 4;
        const int GarrisonStateFortThreshold = 5;
        const int FaithRisenThreshold = 7;

        // ── Tracks ────────────────────────────────────────────────────
        public readonly PoliticalTrack Tyranny = new("Tyranny", 0, 0, 15);
        public readonly PoliticalTrack Faith = new("Faith", 0, 0, 15);
        public readonly PoliticalTrack Fortification = new("Fortification", 0, 0, 15, decayPerDay: 0);
        public readonly PoliticalTrack Humanity = new("Humanity", 5, -10, 20);
        public readonly PoliticalTrack FearLevel = new("Fear", 0, 0, 10);

        // ── Derived Flags ─────────────────────────────────────────────
        // These are computed from track values. Some can also be manually overridden.

        public bool IronFist => Tyranny >= IronFistThreshold;
        public bool PeopleFirst => Faith >= PeopleFirstFaithThreshold && Tyranny <= PeopleFirstTyrannyMax;
        public bool MartialState => Tyranny >= MartialStateTyrannyThreshold;
        public bool GarrisonState => Fortification >= GarrisonStateFortThreshold;
        public bool FaithRisen => Faith >= FaithRisenThreshold;
        public bool MercyDenied => Tyranny >= 5 && Humanity.Value < 0;

        public void Initialize()
        {
            Tyranny.Value = 0;
            Faith.Value = 0;
            Fortification.Value = 0;
            Humanity.Value = 5;
            FearLevel.Value = 0;
        }

        public void ApplyDecay()
        {
            Tyranny.ApplyDecay();
            Faith.ApplyDecay();
            Fortification.ApplyDecay();
            Humanity.ApplyDecay();
            FearLevel.ApplyDecay();
        }
    }
}
