using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MedicalTriageLawHandler : ILawHandler
    {
        readonly MedicalTriageLaw _law;
        readonly IPopupService _popup;

        const double MedicineThreshold = 20;
        const int DailySickDeaths = 3;
        const double DailyMorale = -2;

        public MedicalTriageLawHandler(MedicalTriageLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => state.Medicine < MedicineThreshold;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            int deaths = Math.Min(DailySickDeaths, state.SickWorkers);
            if (deaths > 0)
            {
                state.SickWorkers -= deaths;
                state.TotalDeaths += deaths;
                state.DeathsToday += deaths;
                log.Record("SickWorkers", -deaths, "Medical Triage");
            }

            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Medical Triage");
        }
    }
}
