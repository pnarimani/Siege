using System;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class FireArtisanQuarterEventHandler : IEventHandler
    {
        readonly FireArtisanQuarterEvent _event;

        public string EventId => _event.Id;

        public FireArtisanQuarterEventHandler(FireArtisanQuarterEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.SiegeIntensity >= 3
            && !state.Zones[ZoneId.ArtisanQuarter].IsLost
            && UnityEngine.Random.value < 0.15f;

        public void Execute(GameState state, ChangeLog log)
        {
            state.Materials = Math.Max(0, state.Materials - 40);
            state.Zones[ZoneId.ArtisanQuarter].Integrity =
                Math.Max(0, state.Zones[ZoneId.ArtisanQuarter].Integrity - 12);
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            log.Record("Materials", -40, _event.Name);
            log.Record("ZoneIntegrity:ArtisanQuarter", -12, _event.Name);
            log.Record("TotalDeaths", 1, _event.Name);
            log.Record("DeathsToday", 1, _event.Name);
        }
    }
}
