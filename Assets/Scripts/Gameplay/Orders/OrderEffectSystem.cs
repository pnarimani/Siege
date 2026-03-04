using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Ticks all active toggle orders each day.
    /// </summary>
    public class OrderEffectSystem : ISimulationSystem
    {
        readonly OrderDispatcher _orderDispatcher;
        bool _processedToday;

        public OrderEffectSystem(OrderDispatcher orderDispatcher)
        {
            _orderDispatcher = orderDispatcher;
        }

        public void OnDayStart(GameState state, int day)
        {
            _processedToday = false;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_processedToday) return;
            _processedToday = true;
            _orderDispatcher.TickActiveOrders();
        }
    }
}
