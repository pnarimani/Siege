using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLawHandler : LawHandler<ScorchedEarthLaw>
    {
        const double ImmediateMaterials = -20;
        const double ImmediateUnrest = 5;
        const double DailyUnrest = 5;

        readonly PoliticalState _political;

        public ScorchedEarthLawHandler(ScorchedEarthLaw law, IPopupService popup, PoliticalState political) : base(law, popup)
        {
            _political = political;
        }

        public override bool CanEnact(GameState state) =>
            _political.Fortification.Value >= 3;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Scorched Earth");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Scorched Earth");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Scorched Earth");
        }
    }
}
