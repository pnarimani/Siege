namespace Siege.Gameplay.Laws
{
    public class PurgeTheDisloyalLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "purge_disloyal";
        public string Name => "Purge the Disloyal";
        public string Description => "Root out suspected traitors and make examples of them. Devastating but effective.";
        public string NarrativeText => "The lists were drawn up at midnight. By dawn, eight cells were empty.";
    }
}
