using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    /// <summary>
    /// Base class for all orders. Orders are one-time or toggle actions with cooldowns.
    /// </summary>
    public abstract class Order
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string NarrativeText => null;
        public abstract int CooldownDays { get; }
        public virtual bool IsToggle => false;
        public virtual bool CanDeactivate => true;

        public bool IsActive { get; set; }

        /// <summary>
        /// Check if this order can be issued given current game state.
        /// </summary>
        public abstract bool CanIssue(GameState state);

        /// <summary>
        /// Execute the order's immediate effects.
        /// </summary>
        public abstract void Execute(GameState state, ChangeLog log);

        /// <summary>
        /// For toggle orders: called once per day while active.
        /// </summary>
        public virtual void OnDayTick(GameState state, ChangeLog log) { }
    }
}
