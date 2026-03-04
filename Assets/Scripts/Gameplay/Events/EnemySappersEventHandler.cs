using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemySappersEventHandler : EventHandler<EnemySappersEvent>
    {
        public EnemySappersEventHandler(EnemySappersEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 14;

        public override void Execute(GameState state, ChangeLog log)
        {
            foreach (ZoneId id in Enum.GetValues(typeof(ZoneId)))
            {
                var zone = state.Zones[id];
                if (!zone.IsLost)
                {
                    zone.Integrity -= 5;
                    log.Record($"{id}.Integrity", -5, Event.Name);
                }
            }

            state.SiegeIntensity = Math.Min(state.SiegeIntensity + 1, GameState.MaxSiegeIntensity);
            log.Record("SiegeIntensity", 1, Event.Name);
        }
    }
}
