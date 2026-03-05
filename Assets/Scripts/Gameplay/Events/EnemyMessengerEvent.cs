using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyMessengerEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "enemy_messenger";
        public string Name => "Enemy Messenger";
        public string Description => "A messenger arrives under white flag.\n'Surrender the city, and your people will be spared.'\nYou send him back.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 1) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new EnemyMessengerEvent();
    }
}
