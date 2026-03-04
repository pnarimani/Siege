using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class PublicExecutionsLawHandler : ILawHandler
    {
        readonly PublicExecutionsLaw _law;
        readonly IPopupService _popup;

        const double UnrestThreshold = 60;
        const double ImmediateUnrest = -25;
        const double ImmediateMorale = -20;
        const int ImmediateDeaths = 5;

        public PublicExecutionsLawHandler(PublicExecutionsLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => state.Unrest > UnrestThreshold;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Public Executions");

            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Public Executions");

            state.HealthyWorkers -= ImmediateDeaths;
            state.TotalDeaths += ImmediateDeaths;
            state.DeathsToday += ImmediateDeaths;
            log.Record("HealthyWorkers", -ImmediateDeaths, "Public Executions");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }
    }
}
