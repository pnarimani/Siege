using System;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MedicalTriageLaw : Law
    {
        const double MedicineThreshold = 20;
        const int DailySickDeaths = 3;
        const double DailyMorale = -2;

        public override string Id => "medical_triage";
        public override string Name => "Medical Triage";
        public override string Description => "Abandon the untreatable. Medicine is reserved for those who can still work.";
        public override string NarrativeText => "The physician marks them with chalk. White means medicine. Black means nothing.";

        public override bool CanEnact(GameState state) => state.Medicine < MedicineThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
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
