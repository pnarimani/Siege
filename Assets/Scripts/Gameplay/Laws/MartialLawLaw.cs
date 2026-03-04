namespace Siege.Gameplay.Laws
{
    public class MartialLawLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "martial_law";
        public string Name => "Martial Law";
        public string Description => "Suspend all civil authority. The guards rule now. Unrest is crushed, but at a terrible human cost.";
        public string NarrativeText => "The council chamber is empty. The gallows are not.";
    }
}
