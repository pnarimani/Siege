using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FinalAssaultEvent : IGameEvent
    {
        const int TriggerDay = 33;
        const int AssaultUnrest = 15;

        bool _hasTriggered;

        public string Id => "final_assault";
        public string Name => "Final Assault";
        public string Description => "Battering rams thunder against the gates. The enemy throws everything at the walls. This is the final assault.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.Unrest += AssaultUnrest;
            log.Record("Unrest", AssaultUnrest, Name);
            state.FinalAssaultActive = true;
            log.Record("FinalAssaultActive", 1, Name);
        }

        public IGameEvent Clone() => new FinalAssaultEvent();
    }
}
