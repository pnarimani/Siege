using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BurningFarmsEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "burning_farms";
        public string Name => "Burning Farms";
        public string Description => "Smoke rises beyond the walls. The enemy is burning the farms that once fed this city. There will be no harvest.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 25) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new BurningFarmsEvent();
    }
}
