using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MartialLawLawHandler : LawHandler<MartialLawLaw>
    {
        const double UnrestThreshold = 75;
        const double UnrestCap = 60;
        const double MoraleCap = 35;
        const int DailyExecutions = 2;
        const double DailyFoodCost = -10;

        public MartialLawLawHandler(MartialLawLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("curfew");

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            if (state.Unrest > UnrestCap)
            {
                double reduction = state.Unrest - UnrestCap;
                state.Unrest = UnrestCap;
                log.Record("Unrest", -reduction, "Martial Law (cap)");
            }

            if (state.Morale > MoraleCap)
            {
                double reduction = state.Morale - MoraleCap;
                state.Morale = MoraleCap;
                log.Record("Morale", -reduction, "Martial Law (cap)");
            }

            if (state.HealthyWorkers > 0)
            {
                int deaths = System.Math.Min(DailyExecutions, state.HealthyWorkers);
                state.HealthyWorkers -= deaths;
                state.TotalDeaths += deaths;
                state.DeathsToday += deaths;
                log.Record("HealthyWorkers", -deaths, "Martial Law (executions)");
            }

            state.Food += DailyFoodCost;
            log.Record("Food", DailyFoodCost, "Martial Law (upkeep)");
        }
    }
}
