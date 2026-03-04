using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Wrapper class for all order operations. External systems interact with this, not individual handlers.
    /// Replaces OrderManager.
    /// </summary>
    public class OrderDispatcher
    {
        readonly List<IOrder> _orders;
        readonly Dictionary<string, IOrderHandler> _handlers;
        readonly GameState _state;
        readonly ChangeLog _changeLog;

        public IReadOnlyList<IOrder> AllOrders => _orders;

        public event Action<string> OrderExecuted;

        public OrderDispatcher(IEnumerable<IOrder> orders, IEnumerable<IOrderHandler> handlers, GameState state, ChangeLog changeLog)
        {
            _orders = new List<IOrder>(orders);
            _handlers = new Dictionary<string, IOrderHandler>();
            foreach (var h in handlers)
                _handlers[h.OrderId] = h;
            _state = state;
            _changeLog = changeLog;
        }

        public IOrder GetOrder(string id)
        {
            foreach (var order in _orders)
                if (order.Id == id) return order;
            return null;
        }

        public bool CanIssue(string id)
        {
            if (_state.OrderCooldowns.ContainsKey(id)) return false;
            return _handlers.TryGetValue(id, out var handler) && handler.CanIssue(_state);
        }

        public bool TryExecute(string id)
        {
            if (!_handlers.TryGetValue(id, out var handler)) return false;
            if (_state.OrderCooldowns.ContainsKey(id)) return false;
            if (!handler.CanIssue(_state)) return false;

            var order = GetOrder(id);
            handler.Execute(_state, _changeLog);
            OrderExecuted?.Invoke(id);

            if (order != null && order.CooldownDays > 0)
                _state.OrderCooldowns[id] = order.CooldownDays;

            if (order != null && order.IsToggle)
            {
                order.IsActive = true;
                _state.ActiveToggleOrderIds.Add(id);
            }

            return true;
        }

        public bool TryDeactivate(string id)
        {
            var order = GetOrder(id);
            if (order == null || !order.IsToggle || !order.IsActive) return false;
            if (!order.CanDeactivate) return false;

            order.IsActive = false;
            _state.ActiveToggleOrderIds.Remove(id);
            return true;
        }

        public int GetCooldownRemaining(string id)
        {
            return _state.OrderCooldowns.TryGetValue(id, out var days) ? days : 0;
        }

        public void TickActiveOrders()
        {
            foreach (var order in _orders)
            {
                if (order.IsToggle && order.IsActive && _handlers.TryGetValue(order.Id, out var handler))
                    handler.OnDayTick(_state, _changeLog);
            }
        }
    }
}
