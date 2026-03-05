using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "siege_towers_spotted";
        public string Name => "Siege Towers Spotted";
        public string Description => "Scouts report the enemy has built siege towers. The bombardment will intensify.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 7) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new SiegeTowersSpottedEvent();
    }
}
