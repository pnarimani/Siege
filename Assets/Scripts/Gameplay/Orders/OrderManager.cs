using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Manages all orders: registration, execution, cooldowns, and toggle orders.
    /// </summary>
    public class OrderManager
    {
        readonly GameState _state;
        readonly ChangeLog _changeLog;
        readonly List<Order> _allOrders = new();

        public IReadOnlyList<Order> AllOrders => _allOrders;

        public OrderManager(GameState state, ChangeLog changeLog)
        {
            _state = state;
            _changeLog = changeLog;
            RegisterAllOrders();
        }

        public void Register(Order order) => _allOrders.Add(order);

        public bool TryExecute(string orderId)
        {
            var order = GetOrder(orderId);
            if (order == null) return false;

            // Check cooldown
            if (_state.OrderCooldowns.ContainsKey(orderId)) return false;

            if (!order.CanIssue(_state)) return false;

            order.Execute(_state, _changeLog);

            // Set cooldown
            if (order.CooldownDays > 0)
                _state.OrderCooldowns[orderId] = order.CooldownDays;

            // Activate toggles
            if (order.IsToggle)
            {
                order.IsActive = true;
                _state.ActiveToggleOrderIds.Add(orderId);
            }

            return true;
        }

        public bool TryDeactivate(string orderId)
        {
            var order = GetOrder(orderId);
            if (order == null || !order.IsToggle || !order.IsActive) return false;
            if (!order.CanDeactivate) return false;

            order.IsActive = false;
            _state.ActiveToggleOrderIds.Remove(orderId);
            return true;
        }

        public Order GetOrder(string id)
        {
            foreach (var order in _allOrders)
                if (order.Id == id) return order;
            return null;
        }

        public int GetCooldownRemaining(string id)
        {
            return _state.OrderCooldowns.TryGetValue(id, out var days) ? days : 0;
        }

        void RegisterAllOrders()
        {
            Register(new RationMedicineOrder());
            Register(new ScavengeMedicineOrder());
            Register(new RallyGuardsOrder());
            Register(new ReinforceWallsOrder());
            Register(new StorytellingNightOrder());
            Register(new SacrificeSickOrder());
            Register(new VoluntaryEvacuationOrder());
            Register(new ForcedLaborOrder());
            Register(new InspirePeopleOrder());
            Register(new HoldFeastOrder());
            Register(new QuarantineDistrictOrder());
            Register(new PublicConfessionOrder());
            Register(new PublicTrialOrder());
            Register(new FortifyGateOrder());
            Register(new CrackdownPatrolsOrder());
            Register(new DistributeLuxuriesOrder());
            Register(new DivertSuppliesOrder());
            Register(new BurnSurplusOrder());
            Register(new DayOfRemembranceOrder());
            Register(new BribeEnemyOfficerOrder());
            Register(new HostageExchangeOrder());
            Register(new OfferTributeOrder());
            Register(new SecretCorrespondenceOrder());
            Register(new BetrayAlliesOrder());
        }
    }
}
