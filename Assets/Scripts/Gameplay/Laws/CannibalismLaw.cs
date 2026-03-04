namespace Siege.Gameplay.Laws
{
    public class CannibalismLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "cannibalism";
        public string Name => "Cannibalism";
        public string Description => "The dead shall feed the living. Generates food from deaths but devastates morale and spreads sickness.";
        public string NarrativeText => "No one speaks of where the meat comes from. No one asks.";
    }
}
