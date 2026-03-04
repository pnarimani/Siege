using UnityEngine;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ExtendedShiftsLawHandler : LawHandler<ExtendedShiftsLaw>
    {
        const double ImmediateMorale = -15;
        const double DailySickness = 2;
        const float DeathChance = 0.3f;
        const int DeathCount = 1;

        public ExtendedShiftsLawHandler(ExtendedShiftsLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => true;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Extended Shifts");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
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
