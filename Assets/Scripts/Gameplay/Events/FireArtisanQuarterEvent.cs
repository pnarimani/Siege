using System;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class FireArtisanQuarterEvent : GameEvent
    {
        public override string Id => "fire_artisan_quarter";
        public override string Name => "Fire in the Artisan Quarter";
        public override string Description => "Flames engulf the workshops. The smell of burning timber and lacquer fills the sky.";
        public override bool IsOneTime => true;
        public override int Priority => 60;

        public override bool CanTrigger(GameState state)
        {
            return state.SiegeIntensity >= 3
                && !state.Zones[ZoneId.ArtisanQuarter].IsLost
                && UnityEngine.Random.value < 0.15f;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Materials = Math.Max(0, state.Materials - 40);
            state.Zones[ZoneId.ArtisanQuarter].Integrity =
                Math.Max(0, state.Zones[ZoneId.ArtisanQuarter].Integrity - 12);
            state.TotalDeaths += 1;
            state.DeathsToday += 1;

            log.Record("Materials", -40, Name);
            log.Record("ZoneIntegrity:ArtisanQuarter", -12, Name);
            log.Record("TotalDeaths", 1, Name);
            log.Record("DeathsToday", 1, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
