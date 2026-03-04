using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MartialLawLawHandler : ILawHandler
    {
        readonly MartialLawLaw _law;
        readonly IPopupService _popup;

        const double UnrestThreshold = 75;
        const double UnrestCap = 60;
        const double MoraleCap = 35;
        const int DailyExecutions = 2;
        const double DailyFoodCost = -10;

        public MartialLawLawHandler(MartialLawLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("curfew");

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
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
