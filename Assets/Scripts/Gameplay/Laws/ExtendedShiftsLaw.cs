using UnityEngine;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class ExtendedShiftsLaw : Law
    {
        const double ImmediateMorale = -15;
        const double DailySickness = 2;
        const float DeathChance = 0.3f;
        const int DeathCount = 1;

        public override string Id => "extended_shifts";
        public override string Name => "Extended Shifts";
        public override string Description => "Mandate longer work hours. Boosts production but grinds workers down.";
        public override string NarrativeText => "The hammers do not stop. Neither do the coughs.";

        public override double ProductionMultiplier => 1.25;

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Extended Shifts");
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Extended Shifts");

            if (Random.value < DeathChance && state.HealthyWorkers > 0)
            {
                state.HealthyWorkers -= DeathCount;
                state.TotalDeaths += DeathCount;
                state.DeathsToday += DeathCount;
                log.Record("HealthyWorkers", -DeathCount, "Extended Shifts (accident)");
            }
        }
    }
}
