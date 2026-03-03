using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Ticks all active toggle orders each day.
    /// </summary>
    public class OrderEffectSystem : ISimulationSystem
    {
        readonly OrderManager _orderManager;
        readonly ChangeLog _changeLog;
        bool _processedToday;

        public OrderEffectSystem(OrderManager orderManager, ChangeLog changeLog)
        {
            _orderManager = orderManager;
            _changeLog = changeLog;
        }

        public void OnDayStart(GameState state, int day)
        {
            _processedToday = false;
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_processedToday) return;
            _processedToday = true;

            foreach (var order in _orderManager.AllOrders)
            {
                if (order.IsToggle && order.IsActive)
                    order.OnDayTick(state, _changeLog);
            }
        }
    }
}
