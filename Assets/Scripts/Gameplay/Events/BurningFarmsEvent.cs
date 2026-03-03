using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BurningFarmsEvent : GameEvent
    {
        public override string Id => "burning_farms";
        public override string Name => "Burning Farms";
        public override string Description => "Smoke rises beyond the walls. The enemy is burning the farms that once fed this city. There will be no harvest.";
        public override int Priority => 90;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 25;
    }
}
