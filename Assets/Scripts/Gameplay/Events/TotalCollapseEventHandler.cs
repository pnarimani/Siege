using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TotalCollapseEventHandler : EventHandler<TotalCollapseEvent>
    {
        const int DeficitDaysThreshold = 3;

        public TotalCollapseEventHandler(TotalCollapseEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveFoodDeficitDays >= DeficitDaysThreshold &&
            state.ConsecutiveWaterDeficitDays >= DeficitDaysThreshold;
    }
}
