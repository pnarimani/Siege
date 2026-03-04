namespace Siege.Gameplay.Laws
{
    public class EmergencySheltersLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "emergency_shelters";
        public string Name => "Emergency Shelters";
        public string Description => "Erect makeshift shelters in surviving zones. Increases capacity but worsens overcrowding disease.";
        public string NarrativeText => "Canvas and rope. It is not a home, but it keeps the rain off the children.";
    }
}
