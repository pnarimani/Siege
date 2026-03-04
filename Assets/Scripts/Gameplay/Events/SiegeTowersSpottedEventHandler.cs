using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SiegeTowersSpottedEventHandler : EventHandler<SiegeTowersSpottedEvent>
    {
        public SiegeTowersSpottedEventHandler(SiegeTowersSpottedEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 7;
    }
}
