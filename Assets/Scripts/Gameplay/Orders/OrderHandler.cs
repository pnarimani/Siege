using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public abstract class OrderHandler<T> : IOrderHandler where T : IOrder
    {
        protected readonly T Order;
        protected readonly IPopupService Popup;

        protected OrderHandler(T order, IPopupService popup)
        {
            Order = order;
            Popup = popup;
        }

        public string OrderId => Order.Id;
        public abstract bool CanIssue(GameState state);
        public abstract void Execute(GameState state, ChangeLog log);
        public virtual void OnDayTick(GameState state, ChangeLog log) { }
    }
}
