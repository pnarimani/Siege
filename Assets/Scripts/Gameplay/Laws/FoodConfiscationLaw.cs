namespace Siege.Gameplay.Laws
{
    public class FoodConfiscationLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "food_confiscation";
        public string Name => "Food Confiscation";
        public string Description => "Seize food hoards from the populace. Yields supplies but provokes violence.";
        public string NarrativeText => "The soldiers broke down the baker's door. His family watched.";
    }
}
