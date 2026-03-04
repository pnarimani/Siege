namespace Siege.Gameplay.Laws
{
    public class MandatoryGuardServiceLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "mandatory_guard_service";
        public string Name => "Mandatory Guard Service";
        public string Description => "Compel able-bodied workers into guard service. Bolsters defenses at the cost of morale and food.";
        public string NarrativeText => "They were given spears and told to stand. Most had never held a weapon.";
    }
}
