using UnityEngine;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ExtendedShiftsLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "The hammers do not stop. Neither do the coughs.";
        const double ImmediateMorale = -15;
        const double DailySickness = 2;
        const float DeathChance = 0.3f;
        const int DeathCount = 1;

        public ExtendedShiftsLaw(IPopupService popup) => _popup = popup;

        public string Id => "extended_shifts";
        public string Name => "Extended Shifts";
        public string Description => "Mandate longer work hours. Boosts production but grinds workers down.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.ProductionMultiplier *= 1.25;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Extended Shifts");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
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

        public ILaw Clone() => new ExtendedShiftsLaw(_popup);
    }
}
