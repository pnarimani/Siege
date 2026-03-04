using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CannibalismLawHandler : ILawHandler
    {
        readonly CannibalismLaw _law;
        readonly IPopupService _popup;

        const double FoodThreshold = 40;
        const int MinDeficitDays = 1;
        const double ImmediateUnrest = 20;
        const int DesertionDeaths = 5;
        const int MinFoodFromDead = 1;
        const int MaxFoodFromDead = 10;
        const double DailyMorale = -5;
        const double DailySickness = 3;
        const double DailyUnrest = -3;

        public CannibalismLawHandler(CannibalismLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            state.Food <= FoodThreshold && state.ConsecutiveFoodDeficitDays >= MinDeficitDays;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Cannibalism enacted");

            state.HealthyWorkers -= DesertionDeaths;
            state.TotalDeaths += DesertionDeaths;
            state.DeathsToday += DesertionDeaths;
            log.Record("HealthyWorkers", -DesertionDeaths, "Cannibalism desertions");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            int foodGain = Math.Clamp(state.DeathsToday, MinFoodFromDead, MaxFoodFromDead);
            state.Food += foodGain;
            log.Record("Food", foodGain, "Cannibalism");

            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Cannibalism");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Cannibalism");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Cannibalism");
        }
    }
}
