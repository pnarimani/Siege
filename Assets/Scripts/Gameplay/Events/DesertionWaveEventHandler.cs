using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DesertionWaveEventHandler : EventHandler<DesertionWaveEvent>
    {
        public DesertionWaveEventHandler(DesertionWaveEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.Morale < 30;

        public override void Execute(GameState state, ChangeLog log)
        {
            int lost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= lost;
            log.Record("HealthyWorkers", -lost, Event.Name);
        }
    }
}
