using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class GarrisonMandateLaw : Law
    {
        const int ImmediateConscripts = 3;
        const double ImmediateMorale = -5;
        const double DailyFoodCost = -5;
        const int ConscriptInterval = 3;
        const int PeriodicConscripts = 1;

        int _dayCounter;

        public override string Id => "garrison_mandate";
        public override string Name => "Garrison Mandate";
        public override string Description => "Establish a permanent garrison rotation. Workers are regularly drafted into guard duty.";
        public override string NarrativeText => "Every third sunrise, another name is read from the conscription list.";

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int converted = Math.Min(ImmediateConscripts, state.HealthyWorkers);
            state.HealthyWorkers -= converted;
            state.Guards += converted;
            log.Record("Guards", converted, "Garrison Mandate");
            log.Record("HealthyWorkers", -converted, "Garrison Mandate");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Garrison Mandate");

            _dayCounter = 0;
        }

        public override void OnDayTick(GameState state, ChangeLog log)
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
