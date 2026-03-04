namespace Siege.Gameplay.Orders
{
    public class CrackdownPatrolsOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "crackdown_patrols";
        public string Name => "Crackdown Patrols";
        public string Description => "Send armed patrols to brutally suppress unrest. Effective but costly in lives and morale.";
        public string NarrativeText => "The patrols move through the district at midnight. By morning, the streets are quiet. Three bodies are found.";
        public int CooldownDays => 3;
    }
}
