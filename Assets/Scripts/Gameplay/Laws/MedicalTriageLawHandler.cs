using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MedicalTriageLawHandler : LawHandler<MedicalTriageLaw>
    {
        const double MedicineThreshold = 20;
        const int DailySickDeaths = 3;
        const double DailyMorale = -2;

        public MedicalTriageLawHandler(MedicalTriageLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => state.Medicine < MedicineThreshold;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
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
