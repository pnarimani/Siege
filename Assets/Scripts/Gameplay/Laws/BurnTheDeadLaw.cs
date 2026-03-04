namespace Siege.Gameplay.Laws
{
    public class BurnTheDeadLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "burn_the_dead";
        public string Name => "Burn the Dead";
        public string Description => "Cremate the fallen to halt disease spread. Costs fuel and damages morale.";
        public string NarrativeText => "The pyres burn day and night. The smoke chokes the living, but the plague recedes.";
    }
}
