using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyCommanderLetterEventHandler : EventHandler<EnemyCommanderLetterEvent>
    {
        public EnemyCommanderLetterEventHandler(EnemyCommanderLetterEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 15;
    }
}
