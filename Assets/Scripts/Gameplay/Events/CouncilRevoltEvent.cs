using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CouncilRevoltEvent : IGameEvent
    {
        bool _hasTriggered;

        const double RevoltThreshold = 90.0;

        public string Id => "council_revolt";
        public string Name => "Council Revolt";
        public string Description => "Your reign ends in bloodshed. The council has taken over.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.Unrest <= RevoltThreshold) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new CouncilRevoltEvent();
    }
}
