using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DistantHornsEventHandler : EventHandler<DistantHornsEvent>
    {
        public DistantHornsEventHandler(DistantHornsEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 38;
    }
}
