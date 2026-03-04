namespace Siege.Gameplay.Orders
{
    public class HoldFeastOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "hold_a_feast";
        public string Name => "Hold a Feast";
        public string Description => "A lavish feast burns through supplies but lifts morale and calms unrest.";
        public string NarrativeText => "For one night, the hall glows warm. Children eat until they are full. Tomorrow the cost will be counted.";
        public int CooldownDays => 6;
    }
}
