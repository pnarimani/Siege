using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyMessengerEvent : GameEvent
    {
        public override string Id => "enemy_messenger";
        public override string Name => "Enemy Messenger";
        public override string Description => "A messenger arrives under white flag. 'Surrender the city, and your people will be spared.' You send him back.";
        public override int Priority => 90;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 1;
    }
}
