using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DistantHornsEvent : GameEvent
    {
        public override string Id => "distant_horns";
        public override string Name => "Distant Horns";
        public override string Description => "Horns in the distance. Relief? Or the final assault? You cannot tell.";
        public override int Priority => 90;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 38;
    }
}
