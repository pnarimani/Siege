using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEvent : GameEvent
    {
        public override string Id => "siege_towers_spotted";
        public override string Name => "Siege Towers Spotted";
        public override string Description => "Scouts report the enemy has built siege towers. The bombardment will intensify.";
        public override int Priority => 90;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 7;
    }
}
