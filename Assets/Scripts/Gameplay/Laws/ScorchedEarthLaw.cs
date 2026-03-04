namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "scorched_earth";
        public string Name => "Scorched Earth";
        public string Description => "Burn everything the enemy might use. Reduces siege damage but destroys materials and breeds unrest.";
        public string NarrativeText => "If we cannot hold it, neither shall they. Light the fires.";

        public double SiegeDamageMultiplier => 0.7;
    }
}
