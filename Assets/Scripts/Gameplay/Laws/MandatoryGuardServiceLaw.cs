using System;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MandatoryGuardServiceLaw : ILaw
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const string Narrative = "They were given spears and told to stand. Most had never held a weapon.";
        const double UnrestThreshold = 40;
        const int ImmediateConscripts = 5;
        const double ImmediateMorale = -10;
        const double DailyFoodCost = -4;

        public MandatoryGuardServiceLaw(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "mandatory_guard_service";
        public string Name => "Mandatory Guard Service";
        public string Description => "Compel able-bodied workers into guard service. Bolsters defenses at the cost of morale and food.";

        public bool CanEnact(GameState state) => state.Unrest > UnrestThreshold;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            int converted = Math.Min(ImmediateConscripts, state.HealthyWorkers);
            state.HealthyWorkers -= converted;
            state.Guards += converted;
            log.Record("Guards", converted, "Mandatory Guard Service");
            log.Record("HealthyWorkers", -converted, "Mandatory Guard Service");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Mandatory Guard Service");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Food, 4);
            log.Record("Food", DailyFoodCost, "Mandatory Guard Service (upkeep)");
        }

        public ILaw Clone() => new MandatoryGuardServiceLaw(_popup, _ledger);
    }
}
