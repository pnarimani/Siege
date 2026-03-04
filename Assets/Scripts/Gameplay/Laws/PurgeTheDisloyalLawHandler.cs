using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class PurgeTheDisloyalLawHandler : LawHandler<PurgeTheDisloyalLaw>
    {
        const double ImmediateUnrest = -30;
        const double ImmediateMorale = -15;
        const int ImmediateDeaths = 8;

        readonly PoliticalState _political;

        public PurgeTheDisloyalLawHandler(PurgeTheDisloyalLaw law, IPopupService popup, PoliticalState political) : base(law, popup)
        {
            _political = political;
        }

        public override bool CanEnact(GameState state) =>
            _political.Tyranny.Value >= 6;

        public override void ApplyImmediate(GameState state, ChangeLog log)
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
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }
    }
}
