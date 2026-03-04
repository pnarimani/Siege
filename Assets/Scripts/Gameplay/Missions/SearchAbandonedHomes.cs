namespace Siege.Gameplay.Missions
{
    public class SearchAbandonedHomes : Mission
    {
        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }
        public string Id => "search_abandoned_homes";
        public string Name => "Search Abandoned Homes";
        public string Description => "Scour abandoned dwellings for supplies. Risk of disease.";
        public int DurationDays => 2;
        public int WorkerCost => 4;
    }
}
