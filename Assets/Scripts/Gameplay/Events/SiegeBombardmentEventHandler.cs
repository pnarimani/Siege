using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SiegeBombardmentEventHandler : EventHandler<SiegeBombardmentEvent>
    {
        public SiegeBombardmentEventHandler(SiegeBombardmentEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 8 && UnityEngine.Random.value < 0.20f;

        public override void Execute(GameState state, ChangeLog log)
        {
            var candidates = new List<ZoneId>();
            foreach (ZoneId id in Enum.GetValues(typeof(ZoneId)))
            {
                if (id != ZoneId.Keep && !state.Zones[id].IsLost)
                    candidates.Add(id);
            }

            if (candidates.Count > 0)
            {
                var target = candidates[UnityEngine.Random.Range(0, candidates.Count)];
                double damage = 8 + state.SiegeIntensity * 2;
                state.Zones[target].Integrity = Math.Max(0, state.Zones[target].Integrity - damage);
                log.Record("ZoneIntegrity:" + target, -damage, Event.Name);
            }

            double foodLoss = 5 + state.SiegeIntensity * 3;
            state.Food = Math.Max(0, state.Food - foodLoss);
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            state.WoundedGuards += 2;
            log.Record("Food", -foodLoss, Event.Name);
            log.Record("TotalDeaths", 1, Event.Name);
            log.Record("DeathsToday", 1, Event.Name);
            log.Record("WoundedGuards", 2, Event.Name);
        }
    }
}
