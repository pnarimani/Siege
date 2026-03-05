using System;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MedicalTriageLaw : ILaw
    {
        const string Narrative = "The physician marks them with chalk. White means medicine. Black means nothing.";

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public MedicalTriageLaw(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "medical_triage";
        public string Name => "Medical Triage";
        public string Description => "Abandon the untreatable. Medicine is reserved for those who can still work.";

        public bool CanEnact(GameState state) => _ledger.GetTotal(ResourceType.Medicine) < 20;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            int deaths = Math.Min(3, state.SickWorkers);
            if (deaths > 0)
            {
                state.SickWorkers -= deaths;
                state.DeathsToday += deaths;
                state.TotalDeaths += deaths;
                log.Record("SickWorkers", -deaths, Name);
                log.Record("Deaths", deaths, Name);
            }

            state.Morale -= 2;
            log.Record("Morale", -2, Name);
        }

        public ILaw Clone() => new MedicalTriageLaw(_popup, _ledger);
    }
}
