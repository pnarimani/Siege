namespace Siege.Gameplay.Missions
{
    public class ScavengingLocation
    {
        public string Name;
        public DangerLevel Danger;
        public int MaxVisits;
        public int VisitsRemaining;
        public ResourceQuantity[] PossibleRewards;
        public float CasualtyChance;
        public int MaxCasualties;
    }
}
