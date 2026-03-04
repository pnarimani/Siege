using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ConscriptElderlyLawHandler : LawHandler<ConscriptElderlyLaw>
    {
        const double MoraleCost = -20;
        const double UnrestIncrease = 10;
        const int DailyDeaths = 1;

        public ConscriptElderlyLawHandler(ConscriptElderlyLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => state.Elderly > 0;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            int converted = state.Elderly;
            state.HealthyWorkers += converted;
            state.Elderly = 0;
            log.Record("HealthyWorkers", converted, "Conscript Elderly");
            log.Record("Elderly", -converted, "Conscript Elderly");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Conscript Elderly");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Conscript Elderly");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            if (state.HealthyWorkers <= 0) return;
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= DailyDeaths;
            state.TotalDeaths += DailyDeaths;
            state.DeathsToday += DailyDeaths;
            log.Record("HealthyWorkers", -DailyDeaths, "Conscript Elderly (attrition)");
            Popup.Open(Law.Name, "Another elderly worker collapsed under the strain and did not rise again.", log.SliceSince(before));
        }
    }
}
