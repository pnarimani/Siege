namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "bribe_enemy_officer";
        public string Name => "Bribe Enemy Officer";
        public string Description => "Pay a daily tribute to an enemy officer to reduce siege damage. Risk of interception.";
        public string NarrativeText => "Gold changes hands in the dark. The bombardment eases — but someone may be watching.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
    }
}
