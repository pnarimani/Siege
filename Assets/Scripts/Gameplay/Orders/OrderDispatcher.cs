using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class OrderDispatcher
    {
        readonly List<IOrder> _templates;
        readonly GameState _state;
        readonly ChangeLog _changeLog;

        public IReadOnlyList<IOrder> AllOrders => _templates;

        public event Action<string> OrderExecuted;

        public OrderDispatcher(IEnumerable<IOrder> orders, GameState state, ChangeLog changeLog)
        {
            _templates = new List<IOrder>(orders);
            _state = state;
            _changeLog = changeLog;
        }

        public IOrder GetOrder(string id)
        {
            foreach (var order in _templates)
                if (order.Id == id) return order;
            return null;
        }

        public bool CanIssue(string id)
        {
            if (_state.OrderCooldowns.ContainsKey(id)) return false;
            var order = GetOrder(id);
            return order != null && order.CanIssue(_state);
        }

        public bool TryExecute(string id)
        {
            if (_state.ActionUsedToday) return false;
            if (_state.OrderCooldowns.ContainsKey(id)) return false;
            var template = GetOrder(id);
            if (template == null || !template.CanIssue(_state)) return false;

            var copy = template.Clone();
            copy.OnExecute(_state, _changeLog);
            _state.ActionUsedToday = true;
            OrderExecuted?.Invoke(id);

            if (template.CooldownDays > 0)
                _state.OrderCooldowns[id] = template.CooldownDays;

            return true;
        }

        public int GetCooldownRemaining(string id)
        {
            return _state.OrderCooldowns.TryGetValue(id, out var days) ? days : 0;
        }
    }
}
