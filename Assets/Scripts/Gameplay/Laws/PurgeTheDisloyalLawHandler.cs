using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class PurgeTheDisloyalLawHandler : ILawHandler
    {
        readonly PurgeTheDisloyalLaw _law;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const double ImmediateUnrest = -30;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 8;

        public PurgeTheDisloyalLawHandler(PurgeTheDisloyalLaw law, IPopupService popup, PoliticalState political)
        {
            _law = law;
            _popup = popup;
            _political = political;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            _political.Tyranny.Value >= 6;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Purge the Disloyal");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Purge the Disloyal");

            state.HealthyWorkers -= ImmediateDeaths;
            state.TotalDeaths += ImmediateDeaths;
            state.DeathsToday += ImmediateDeaths;
            log.Record("HealthyWorkers", -ImmediateDeaths, "Purge the Disloyal");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }
    }
}
