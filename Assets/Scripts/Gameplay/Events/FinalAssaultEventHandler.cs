using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FinalAssaultEventHandler : EventHandler<FinalAssaultEvent>
    {
        public FinalAssaultEventHandler(FinalAssaultEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 33;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Unrest += 15;
            log.Record("Unrest", 15, Event.Name);
            state.FinalAssaultActive = true;
            log.Record("FinalAssaultActive", 1, Event.Name);
        }
    }
}
