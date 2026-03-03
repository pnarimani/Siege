using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    public abstract class Mission
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int DurationDays { get; }
        public abstract int WorkerCost { get; }
        public virtual int GuardCost => 0;

        public bool IsActive { get; set; }
        public int DaysRemaining { get; set; }

        public abstract bool CanLaunch(GameState state);
        public abstract MissionOutcome Resolve(GameState state, ChangeLog log);
    }
}
