using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FinalAssaultEvent : GameEvent
    {
        public override string Id => "final_assault";
        public override string Name => "Final Assault";
        public override string Description => "Battering rams thunder against the gates. The enemy throws everything at the walls. This is the final assault.";
        public override int Priority => 100;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 33;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Unrest += 15;
            log.Record("Unrest", 15, Name);

            state.FinalAssaultActive = true;
            log.Record("FinalAssaultActive", 1, Name);
        }
    }
}
