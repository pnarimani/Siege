using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyCommanderLetterEvent : GameEvent
    {
        public override string Id => "enemy_commander_letter";
        public override string Name => "Enemy Commander's Letter";
        public override string Description => "A letter from the enemy commander: 'Your walls weaken. Your people starve. How long will you sacrifice them for pride?'";
        public override int Priority => 90;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 15;
    }
}
