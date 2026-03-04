using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MandatoryGuardServiceLaw : Law
    {
        const double UnrestThreshold = 40;
        const int ImmediateConscripts = 5;
        const double ImmediateMorale = -10;
        const double DailyFoodCost = -4;

        public override string Id => "mandatory_guard_service";
        public override string Name => "Mandatory Guard Service";
        public override string Description => "Compel able-bodied workers into guard service. Bolsters defenses at the cost of morale and food.";
        public override string NarrativeText => "They were given spears and told to stand. Most had never held a weapon.";

        public override bool CanEnact(GameState state) => state.Unrest > UnrestThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            int converted = Math.Min(ImmediateConscripts, state.HealthyWorkers);
            state.HealthyWorkers -= converted;
            state.Guards += converted;
            log.Record("Guards", converted, "Mandatory Guard Service");
            log.Record("HealthyWorkers", -converted, "Mandatory Guard Service");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Mandatory Guard Service");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food += DailyFoodCost;
            log.Record("Food", DailyFoodCost, "Mandatory Guard Service (upkeep)");
        }
    }
}
