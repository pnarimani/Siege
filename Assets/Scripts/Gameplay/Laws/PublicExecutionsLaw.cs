namespace Siege.Gameplay.Laws
{
    public class PublicExecutionsLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "public_executions";
        public string Name => "Public Executions";
        public string Description => "Execute dissidents publicly to restore order. Crushes unrest but shatters morale.";
        public string NarrativeText => "The crowd watches in silence. Fear is a kind of obedience.";
    }
}
