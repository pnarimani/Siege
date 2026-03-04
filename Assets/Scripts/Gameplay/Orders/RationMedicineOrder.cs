namespace Siege.Gameplay.Orders
{
    public class RationMedicineOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "ration_medicine";
        public string Name => "Ration Medicine";
        public string Description => "Distribute medicine rations to the sick, reducing sickness at the cost of unrest.";
        public string NarrativeText => "The sick line up in the cold, clutching their numbered tokens. There is never enough.";
        public int CooldownDays => 3;
    }
}
