namespace Siege.Gameplay.Laws
{
    public class ConscriptElderlyLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "conscript_elderly";
        public string Name => "Conscript the Elderly";
        public string Description => "Draft the elderly into the workforce. They will work, but they will not last.";
        public string NarrativeText => "Grandfather picked up a shovel today. He did not put it down.";
    }
}
