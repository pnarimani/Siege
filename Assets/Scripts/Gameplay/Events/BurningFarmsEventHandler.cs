using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BurningFarmsEventHandler : EventHandler<BurningFarmsEvent>
    {
        public BurningFarmsEventHandler(BurningFarmsEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 25;
    }
}
