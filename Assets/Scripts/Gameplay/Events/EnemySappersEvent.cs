using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemySappersEvent : GameEvent
    {
        public override string Id => "enemy_sappers";
        public override string Name => "Enemy Sappers";
        public override string Description => "Enemy miners have been at work beneath the walls. The ground shudders as tunnels collapse, weakening every district.";
        public override int Priority => 80;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 14;

        public override void Execute(GameState state, ChangeLog log)
        {
            foreach (ZoneId id in Enum.GetValues(typeof(ZoneId)))
            {
                var zone = state.Zones[id];
                if (!zone.IsLost)
                {
                    zone.Integrity -= 5;
                    log.Record($"{id}.Integrity", -5, Name);
                }
            }

            state.SiegeIntensity = Math.Min(state.SiegeIntensity + 1, GameState.MaxSiegeIntensity);
            log.Record("SiegeIntensity", 1, Name);
        }
    }
}
