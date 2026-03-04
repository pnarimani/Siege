namespace Siege.Gameplay.Laws
{
    public class MedicalTriageLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "medical_triage";
        public string Name => "Medical Triage";
        public string Description => "Abandon the untreatable. Medicine is reserved for those who can still work.";
        public string NarrativeText => "The physician marks them with chalk. White means medicine. Black means nothing.";
    }
}
