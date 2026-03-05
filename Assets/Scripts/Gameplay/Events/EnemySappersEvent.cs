using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemySappersEvent : IGameEvent
    {
        const int TriggerDay = 14;
        const int IntegrityDamagePerZone = 5;

        bool _hasTriggered;

        public string Id => "enemy_sappers";
        public string Name => "Enemy Sappers";
        public string Description => "Enemy miners have been at work beneath the walls. The ground shudders as tunnels collapse, weakening every district.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            foreach (ZoneId id in ZoneIds.All)
            {
                var zone = state.Zones[id];
                if (!zone.IsLost)
                {
                    zone.Integrity -= IntegrityDamagePerZone;
                    log.Record($"{id}.Integrity", -IntegrityDamagePerZone, Name);
                }
            }

            state.SiegeIntensity = System.Math.Min(state.SiegeIntensity + 1, GameState.MaxSiegeIntensity);
            log.Record("SiegeIntensity", 1, Name);
        }

        public IGameEvent Clone() => new EnemySappersEvent();
    }
}
