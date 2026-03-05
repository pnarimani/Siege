using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class GarrisonMandateLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "Every third sunrise, another name is read from the conscription list.";
        const int ImmediateConscripts = 3;
        const double ImmediateMorale = -5;
        const double DailyFoodCost = -5;
        const int ConscriptInterval = 3;
        const int PeriodicConscripts = 1;

        int _dayCounter;

        public GarrisonMandateLaw(IPopupService popup)
        {
            _popup = popup;
            _dayCounter = 0;
        }

        public string Id => "garrison_mandate";
        public string Name => "Garrison Mandate";
        public string Description => "Establish a permanent garrison rotation. Workers are regularly drafted into guard duty.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
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

        public ILaw Clone() => new GarrisonMandateLaw(_popup);
    }
}
