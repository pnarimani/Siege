using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLawHandler : ILawHandler
    {
        readonly ScorchedEarthLaw _law;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const double ImmediateMaterials = -20;
        const double ImmediateUnrest = 5;
        const double DailyUnrest = 5;

        public ScorchedEarthLawHandler(ScorchedEarthLaw law, IPopupService popup, PoliticalState political)
        {
            _law = law;
            _popup = popup;
            _political = political;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            _political.Fortification.Value >= 3;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Scorched Earth");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Scorched Earth");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Scorched Earth");
        }
    }
}
