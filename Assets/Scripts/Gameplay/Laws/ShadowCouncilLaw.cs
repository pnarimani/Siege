namespace Siege.Gameplay.Laws
{
    public class ShadowCouncilLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "shadow_council";
        public string Name => "Shadow Council";
        public string Description => "Cede power to a secretive inner circle. They keep order through disappearances.";
        public string NarrativeText => "No one knows who sits on the council. That is the point.";

        public double ProductionMultiplier => 1.05;
    }
}
