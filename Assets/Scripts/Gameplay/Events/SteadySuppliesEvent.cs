using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SteadySuppliesEvent : GameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "steady_supplies";
        public string Name => "Steady Supplies";
        public string Description => "Consistent supply flow boosts morale.";
        public int Priority => 20;
        public bool IsOneTime => false;

        public string GetNarrativeText(GameState state) =>
            "The steady flow of food and water lifts spirits across the city.";
    }
}
