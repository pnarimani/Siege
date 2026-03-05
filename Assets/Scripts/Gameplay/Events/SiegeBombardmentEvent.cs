using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SiegeBombardmentEvent : IGameEvent
    {
        const int StartDay = 8;
        const float TriggerChance = 0.20f;
        const int BaseDamage = 8;
        const int DamagePerIntensity = 2;
        const int BaseFoodLoss = 5;
        const int FoodLossPerIntensity = 3;
        const int WoundedGuardsPerBombardment = 2;

        public string Id => "siege_bombardment";
        public string Name => "Siege Bombardment";
        public string Description => "The active perimeter sustains damage from bombardment. Stone and flame rain down without warning.";

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= StartDay && Random.value < TriggerChance;

        public void Execute(GameState state, ChangeLog log)
        {
            using var candidates = TempList<ZoneId>.Get();
            foreach (ZoneId id in ZoneIds.All)
            {
                if (id != ZoneId.Keep && !state.Zones[id].IsLost)
                    candidates.Add(id);
            }
            if (candidates.Count > 0)
            {
                var target = candidates[Random.Range(0, candidates.Count)];
                double damage = BaseDamage + state.SiegeIntensity * DamagePerIntensity;
                state.Zones[target].Integrity = System.Math.Max(0, state.Zones[target].Integrity - damage);
                log.Record("ZoneIntegrity:" + target, -damage, Name);
            }
            double foodLoss = BaseFoodLoss + state.SiegeIntensity * FoodLossPerIntensity;
            state.Food = System.Math.Max(0, state.Food - foodLoss);
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            state.WoundedGuards += WoundedGuardsPerBombardment;
            log.Record("Food", -foodLoss, Name);
            log.Record("TotalDeaths", 1, Name);
            log.Record("DeathsToday", 1, Name);
            log.Record("WoundedGuards", WoundedGuardsPerBombardment, Name);
        }

        public IGameEvent Clone() => new SiegeBombardmentEvent();
    }
}
