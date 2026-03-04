using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CouncilRevoltEventHandler : EventHandler<CouncilRevoltEvent>
    {
        const double RevoltThreshold = 90.0;

        public CouncilRevoltEventHandler(CouncilRevoltEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.Unrest > RevoltThreshold;
    }
}
