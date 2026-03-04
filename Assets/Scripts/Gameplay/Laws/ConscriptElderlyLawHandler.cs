using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ConscriptElderlyLawHandler : ILawHandler
    {
        readonly ConscriptElderlyLaw _law;
        readonly IPopupService _popup;

        const double MoraleCost = -20;
        const double UnrestIncrease = 10;
        const int DailyDeaths = 1;

        public ConscriptElderlyLawHandler(ConscriptElderlyLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => state.Elderly > 0;

        public void ApplyImmediate(GameState state, ChangeLog log)
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
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            if (state.HealthyWorkers <= 0) return;
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= DailyDeaths;
            state.TotalDeaths += DailyDeaths;
            state.DeathsToday += DailyDeaths;
            log.Record("HealthyWorkers", -DailyDeaths, "Conscript Elderly (attrition)");
            _popup.Open(_law.Name, "Another elderly worker collapsed under the strain and did not rise again.", log.SliceSince(before));
        }
    }
}
