namespace Siege.Gameplay.Laws
{
    public class CurfewLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "curfew";
        public string Name => "Curfew";
        public string Description => "Enforce a nightly curfew. Reduces unrest but slows production.";
        public string NarrativeText => "After dark, only guards walk the streets.";

        public double ProductionMultiplier => 0.85;
    }
}
