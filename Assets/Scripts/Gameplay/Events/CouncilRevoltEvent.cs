using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CouncilRevoltEvent : GameEvent
    {
        const double RevoltThreshold = 90.0;

        public override string Id => "council_revolt";
        public override string Name => "Council Revolt";
        public override string Description => "The council has seized control.";
        public override int Priority => 200;

        public override bool CanTrigger(GameState state) =>
            state.Unrest > RevoltThreshold;

        public override string GetNarrativeText(GameState state) =>
            "Your reign ends in bloodshed. The council has taken over.";
    }
}
