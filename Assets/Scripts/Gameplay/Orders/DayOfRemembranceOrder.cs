namespace Siege.Gameplay.Orders
{
    public class DayOfRemembranceOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "day_of_remembrance";
        public string Name => "Day of Remembrance";
        public string Description => "Honor the fallen with a day of mourning. Lifts morale but gatherings spread illness.";
        public string NarrativeText => "Names are read aloud until the sun sets. The living weep for the dead and find strength in grief.";
        public int CooldownDays => 10;
    }
}
