using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DesertionWaveEvent : GameEvent
    {
        public override string Id => "desertion_wave";
        public override string Name => "Desertion Wave";
        public override string Description => "At dawn, the western gate stands open. Footprints in the mud lead away into the fog.";
        public override bool IsOneTime => true;
        public override int Priority => 70;

        public override bool CanTrigger(GameState state)
        {
            return state.Morale < 30;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            int lost = Math.Min(10, state.HealthyWorkers);
            state.HealthyWorkers -= lost;

            log.Record("HealthyWorkers", -lost, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
