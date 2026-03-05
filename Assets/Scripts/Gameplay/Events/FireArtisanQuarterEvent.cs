using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class FireArtisanQuarterEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "fire_artisan_quarter";
        public string Name => "Fire in the Artisan Quarter";
        public string Description => "Flames engulf the workshops. The smell of burning timber and lacquer fills the sky.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.SiegeIntensity >= 3
                && !state.Zones[ZoneId.ArtisanQuarter].IsLost
                && Random.value < 0.15f)
            {
                _hasTriggered = true;
                return true;
            }
            return false;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.Materials = System.Math.Max(0, state.Materials - 40);
            state.Zones[ZoneId.ArtisanQuarter].Integrity =
                System.Math.Max(0, state.Zones[ZoneId.ArtisanQuarter].Integrity - 12);
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            log.Record("Materials", -40, Name);
            log.Record("ZoneIntegrity:ArtisanQuarter", -12, Name);
            log.Record("TotalDeaths", 1, Name);
            log.Record("DeathsToday", 1, Name);
        }

        public IGameEvent Clone() => new FireArtisanQuarterEvent();
    }
}
