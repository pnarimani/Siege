using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DesertionWaveEventHandler : IEventHandler
    {
        readonly DesertionWaveEvent _event;

        public string EventId => _event.Id;

        public DesertionWaveEventHandler(DesertionWaveEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.Morale < 30;

        public void Execute(GameState state, ChangeLog log)
        {
            int lost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= lost;
            log.Record("HealthyWorkers", -lost, _event.Name);
        }
    }
}
