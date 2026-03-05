using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyCommanderLetterEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "enemy_commander_letter";
        public string Name => "Enemy Commander's Letter";
        public string Description => "A letter from the enemy commander: 'Your walls weaken. Your people starve. How long will you sacrifice them for pride?'";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 15) return false;
            _hasTriggered = true;
            return true;
        }

        public IGameEvent Clone() => new EnemyCommanderLetterEvent();
    }
}
