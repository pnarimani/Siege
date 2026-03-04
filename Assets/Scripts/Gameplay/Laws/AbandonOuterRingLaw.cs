namespace Siege.Gameplay.Laws
{
    public class AbandonOuterRingLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "abandon_outer_ring";
        public string Name => "Abandon the Outer Ring";
        public string Description => "Withdraw all forces from the outer farms, ceding them to the enemy. Reduces siege pressure but causes unrest.";
        public string NarrativeText => "The outer fields are lost. Pull everyone back behind the second wall — and pray it holds.";

        public double SiegeDamageMultiplier => 0.8;
    }
}
