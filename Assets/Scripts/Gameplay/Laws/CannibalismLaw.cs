using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CannibalismLaw : Law
    {
        const double FoodThreshold = 40;
        const int MinDeficitDays = 1;
        const double ImmediateUnrest = 20;
        const int DesertionDeaths = 5;
        const int MinFoodFromDead = 1;
        const int MaxFoodFromDead = 10;
        const double DailyMorale = -5;
        const double DailySickness = 3;
        const double DailyUnrest = -3;

        public override string Id => "cannibalism";
        public override string Name => "Cannibalism";
        public override string Description => "The dead shall feed the living. Generates food from deaths but devastates morale and spreads sickness.";
        public override string NarrativeText => "No one speaks of where the meat comes from. No one asks.";

        public override bool CanEnact(GameState state) =>
            state.Food <= FoodThreshold && state.ConsecutiveFoodDeficitDays >= MinDeficitDays;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Cannibalism enacted");

            state.HealthyWorkers -= DesertionDeaths;
            state.TotalDeaths += DesertionDeaths;
            state.DeathsToday += DesertionDeaths;
            log.Record("HealthyWorkers", -DesertionDeaths, "Cannibalism desertions");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
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
