using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    /// <summary>
    /// Base class for all laws. Laws are permanent once enacted.
    /// Each law defines its own conditions, immediate effects, and daily effects.
    /// </summary>
    public abstract class Law
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string NarrativeText => null;

        public bool IsEnacted { get; private set; }

        /// <summary>
        /// Check if this law can be enacted given current game state.
        /// </summary>
        public abstract bool CanEnact(GameState state);

        /// <summary>
        /// Apply immediate effects when the law is enacted.
        /// </summary>
        protected abstract void ApplyImmediate(GameState state, ChangeLog log);

        /// <summary>
        /// Apply ongoing daily effects. Called once per day for enacted laws.
        /// Override only if the law has daily effects.
        /// </summary>
        public virtual void OnDayTick(GameState state, ChangeLog log) { }

        /// <summary>
        /// Returns a production multiplier modifier (e.g., 0.85 for -15%, 1.25 for +25%).
        /// Default is 1.0 (no change).
        /// </summary>
        public virtual double ProductionMultiplier => 1.0;

        /// <summary>
        /// Returns a food consumption multiplier modifier.
        /// </summary>
        public virtual double FoodConsumptionMultiplier => 1.0;

        /// <summary>
        /// Returns a water consumption multiplier modifier.
        /// </summary>
        public virtual double WaterConsumptionMultiplier => 1.0;

        /// <summary>
        /// Returns a siege damage multiplier modifier.
        /// </summary>
        public virtual double SiegeDamageMultiplier => 1.0;

        public void Enact(GameState state, ChangeLog log)
        {
            if (IsEnacted) return;
            if (!CanEnact(state)) return;

            IsEnacted = true;
            state.EnactedLawIds.Add(Id);
            ApplyImmediate(state, log);
        }
    }
}
