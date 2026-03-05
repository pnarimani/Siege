using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CannibalismLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "No one speaks of where the meat comes from. No one asks.";
        const double FoodThreshold = 40;
        const int MinDeficitDays = 1;
        const double ImmediateUnrest = 20;
        const int DesertionDeaths = 5;
        const int MinFoodFromDead = 1;
        const int MaxFoodFromDead = 10;
        const double DailyMorale = -5;
        const double DailySickness = 3;
        const double DailyUnrest = -3;

        public CannibalismLaw(IPopupService popup) => _popup = popup;

        public string Id => "cannibalism";
        public string Name => "Cannibalism";
        public string Description => "The dead shall feed the living. Generates food from deaths but devastates morale and spreads sickness.";

        public bool CanEnact(GameState state) =>
            state.Food <= FoodThreshold && state.ConsecutiveFoodDeficitDays >= MinDeficitDays;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Cannibalism enacted");

            state.HealthyWorkers -= DesertionDeaths;
            state.TotalDeaths += DesertionDeaths;
            state.DeathsToday += DesertionDeaths;
            log.Record("HealthyWorkers", -DesertionDeaths, "Cannibalism desertions");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
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

        public ILaw Clone() => new CannibalismLaw(_popup);
    }
}
