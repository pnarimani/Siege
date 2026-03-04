using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MandatoryGuardServiceLawHandler : LawHandler<MandatoryGuardServiceLaw>
    {
        const double UnrestThreshold = 40;
        const int ImmediateConscripts = 5;
        const double ImmediateMorale = -10;
        const double DailyFoodCost = -4;

        public MandatoryGuardServiceLawHandler(MandatoryGuardServiceLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => state.Unrest > UnrestThreshold;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            int converted = Math.Min(ImmediateConscripts, state.HealthyWorkers);
            state.HealthyWorkers -= converted;
            state.Guards += converted;
            log.Record("Guards", converted, "Mandatory Guard Service");
            log.Record("HealthyWorkers", -converted, "Mandatory Guard Service");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Mandatory Guard Service");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food += DailyFoodCost;
            log.Record("Food", DailyFoodCost, "Mandatory Guard Service (upkeep)");
        }
    }
}
