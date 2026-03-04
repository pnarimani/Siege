namespace Siege.Gameplay.Orders
{
    public class VoluntaryEvacuationOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "voluntary_evacuation";
        public string Name => "Voluntary Evacuation";
        public string Description => "Abandon the outermost zone, pulling the perimeter inward.";
        public string NarrativeText => "Families carry what they can. Behind them, the district falls silent.";
        public int CooldownDays => 0;
    }
}
