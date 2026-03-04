using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DespairEventHandler : IEventHandler
    {
        readonly DespairEvent _event;

        public string EventId => _event.Id;

        public DespairEventHandler(DespairEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 10 && state.Morale < 45 && Random.value < 0.15f;

        public void Execute(GameState state, ChangeLog log)
        {
            state.Morale -= 10;
            state.Unrest += 8;
            log.Record("Morale", -10, _event.Name);
            log.Record("Unrest", 8, _event.Name);
        }
    }
}
