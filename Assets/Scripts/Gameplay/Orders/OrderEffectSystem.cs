using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Placeholder for any future per-day order effects.
    /// </summary>
    public class OrderEffectSystem : ISimulationSystem
    {
        public OrderEffectSystem(OrderDispatcher orderDispatcher) { }

        public void OnDayStart(GameState state, int day) { }

        public void Tick(GameState state, float deltaTime) { }
    }
}
