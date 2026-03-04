using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemySappersEventHandler : IEventHandler
    {
        const int TriggerDay = 14;
        const int IntegrityDamagePerZone = 5;

        readonly EnemySappersEvent _event;

        public string EventId => _event.Id;

        public EnemySappersEventHandler(EnemySappersEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void Execute(GameState state, ChangeLog log)
        {
            foreach (ZoneId id in ZoneIds.All)
            {
                var zone = state.Zones[id];
                if (!zone.IsLost)
                {
                    zone.Integrity -= IntegrityDamagePerZone;
                    log.Record($"{id}.Integrity", -IntegrityDamagePerZone, _event.Name);
                }
            }

            state.SiegeIntensity = Math.Min(state.SiegeIntensity + 1, GameState.MaxSiegeIntensity);
            log.Record("SiegeIntensity", 1, _event.Name);
        }
    }
}
