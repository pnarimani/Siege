using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class GarrisonMandateLawHandler : ILawHandler
    {
        readonly GarrisonMandateLaw _law;
        readonly IPopupService _popup;

        const int ImmediateConscripts = 3;
        const double ImmediateMorale = -5;
        const double DailyFoodCost = -5;
        const int ConscriptInterval = 3;
        const int PeriodicConscripts = 1;

        int _dayCounter;

        public GarrisonMandateLawHandler(GarrisonMandateLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
            _dayCounter = 0;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => true;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            int converted = Math.Min(ImmediateConscripts, state.HealthyWorkers);
            state.HealthyWorkers -= converted;
            state.Guards += converted;
            log.Record("Guards", converted, "Garrison Mandate");
            log.Record("HealthyWorkers", -converted, "Garrison Mandate");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Garrison Mandate");

            _dayCounter = 0;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food += DailyFoodCost;
            log.Record("Food", DailyFoodCost, "Garrison Mandate (upkeep)");

            _dayCounter++;
            if (_dayCounter >= ConscriptInterval && state.HealthyWorkers > 0)
            {
                _dayCounter = 0;
                int converted = Math.Min(PeriodicConscripts, state.HealthyWorkers);
                state.HealthyWorkers -= converted;
                state.Guards += converted;
                log.Record("Guards", converted, "Garrison Mandate (draft)");
                log.Record("HealthyWorkers", -converted, "Garrison Mandate (draft)");
            }
        }
    }
}
