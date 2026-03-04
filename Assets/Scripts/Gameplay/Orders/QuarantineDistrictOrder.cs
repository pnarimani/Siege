namespace Siege.Gameplay.Orders
{
    public class QuarantineDistrictOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "quarantine_district";
        public string Name => "Quarantine District";
        public string Description => "Seal off a diseased area to contain the outbreak.";
        public string NarrativeText => "The barricades go up. Behind them, the coughing continues — but it does not spread.";
        public int CooldownDays => 3;
    }
}
