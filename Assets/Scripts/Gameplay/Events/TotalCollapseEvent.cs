using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TotalCollapseEvent : GameEvent
    {
        const int DeficitDaysThreshold = 3;

        public override string Id => "total_collapse";
        public override string Name => "Total Collapse";
        public override string Description => "All supplies are exhausted. The city collapses.";
        public override int Priority => 200;

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveFoodDeficitDays >= DeficitDaysThreshold &&
            state.ConsecutiveWaterDeficitDays >= DeficitDaysThreshold;

        public override string GetNarrativeText(GameState state) =>
            "The last rations are gone. The last barrels are dry. " +
            "People collapse in the streets.";
    }
}
