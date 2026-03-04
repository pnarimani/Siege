namespace Siege.Gameplay.Laws
{
    public class GarrisonMandateLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "garrison_mandate";
        public string Name => "Garrison Mandate";
        public string Description => "Establish a permanent garrison rotation. Workers are regularly drafted into guard duty.";
        public string NarrativeText => "Every third sunrise, another name is read from the conscription list.";
    }
}
