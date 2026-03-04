namespace Siege.Gameplay.Laws
{
    public class FaithProcessionsLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "faith_processions";
        public string Name => "Faith Processions";
        public string Description => "Organize daily religious processions through the streets. Lifts spirits but risks spreading disease.";
        public string NarrativeText => "They sing as they walk. For a moment, the walls do not feel so close.";
    }
}
