using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ConscriptElderlyLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "Grandfather picked up a shovel today. He did not put it down.";
        const double MoraleCost = -20;
        const double UnrestIncrease = 10;
        const int DailyDeaths = 1;

        public ConscriptElderlyLaw(IPopupService popup) => _popup = popup;

        public string Id => "conscript_elderly";
        public string Name => "Conscript the Elderly";
        public string Description => "Draft the elderly into the workforce. They will work, but they will not last.";

        public bool CanEnact(GameState state) => state.Elderly > 0;

        public void OnEnact(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            if (state.HealthyWorkers <= 0) return;
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= DailyDeaths;
            state.TotalDeaths += DailyDeaths;
            state.DeathsToday += DailyDeaths;
            log.Record("HealthyWorkers", -DailyDeaths, "Conscript Elderly (attrition)");
            _popup.Open(Name, "Another elderly worker collapsed under the strain and did not rise again.", log.SliceSince(before));
        }

        public ILaw Clone() => new ConscriptElderlyLaw(_popup);
    }
}
