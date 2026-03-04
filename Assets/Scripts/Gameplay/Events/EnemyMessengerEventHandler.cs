using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyMessengerEventHandler : EventHandler<EnemyMessengerEvent>
    {
        public EnemyMessengerEventHandler(EnemyMessengerEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 1;
    }
}
