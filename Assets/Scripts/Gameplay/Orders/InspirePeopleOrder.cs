namespace Siege.Gameplay.Orders
{
    public class InspirePeopleOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "inspire_people";
        public string Name => "Inspire the People";
        public string Description => "Spend food and water to rally the populace, lifting spirits significantly.";
        public string NarrativeText => "A leader climbs the barricade and speaks. For the first time in days, people cheer.";
        public int CooldownDays => 4;
    }
}
