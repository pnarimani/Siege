using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DistantHornsEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "distant_horns";
        public string Name => "Distant Horns";
        public string Description => "Horns in the distance. Relief? Or the final assault? You cannot tell.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 38) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new DistantHornsEvent();
    }
}
