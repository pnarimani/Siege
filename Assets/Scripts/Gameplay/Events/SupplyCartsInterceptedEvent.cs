using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SupplyCartsInterceptedEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "supply_carts_intercepted";
        public string Name => "Supply Carts Intercepted";
        public string Description => "Enemy raiders have intercepted supply carts bound for the city.";
        public int Priority => 50;
        public bool IsOneTime => false;

        public bool LostFood { get; set; }


        public string GetNarrativeText(GameState state) =>
            LostFood
                ? "Enemy raiders intercepted a food convoy. Precious grain lost to the siege lines."
                : "Enemy raiders seized water barrels en route to the city. Our reserves shrink.";
    }
}
