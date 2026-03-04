using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FinalAssaultEventHandler : IEventHandler
    {
        const int TriggerDay = 33;
        const int AssaultUnrest = 15;

        readonly FinalAssaultEvent _event;

        public string EventId => _event.Id;

        public FinalAssaultEventHandler(FinalAssaultEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void Execute(GameState state, ChangeLog log)
        {
            state.Unrest += AssaultUnrest;
            log.Record("Unrest", AssaultUnrest, _event.Name);
            state.FinalAssaultActive = true;
            log.Record("FinalAssaultActive", 1, _event.Name);
        }
    }
}
