using UnityEngine;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ExtendedShiftsLawHandler : ILawHandler
    {
        readonly ExtendedShiftsLaw _law;
        readonly IPopupService _popup;

        const double ImmediateMorale = -15;
        const double DailySickness = 2;
        const float DeathChance = 0.3f;
        const int DeathCount = 1;

        public ExtendedShiftsLawHandler(ExtendedShiftsLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => true;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Extended Shifts");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
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
