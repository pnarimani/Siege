namespace Siege.Gameplay.Orders
{
    public class ReinforceWallsOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "reinforce_walls";
        public string Name => "Reinforce Walls";
        public string Description => "Spend materials to shore up the perimeter walls.";
        public string NarrativeText => "Workers haul rubble and timber through the night. The wall holds — for now.";
        public int CooldownDays => 3;
    }
}
