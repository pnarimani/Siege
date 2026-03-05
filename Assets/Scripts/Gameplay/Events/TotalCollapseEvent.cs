using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TotalCollapseEvent : IGameEvent
    {
        bool _hasTriggered;

        const int DeficitDaysThreshold = 3;

        public string Id => "total_collapse";
        public string Name => "Total Collapse";
        public string Description => "The last rations are gone. The last barrels are dry. People collapse in the streets.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.ConsecutiveFoodDeficitDays < DeficitDaysThreshold ||
                state.ConsecutiveWaterDeficitDays < DeficitDaysThreshold) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new TotalCollapseEvent();
    }
}
