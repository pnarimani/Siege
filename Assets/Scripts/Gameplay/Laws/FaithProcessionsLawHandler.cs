using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class FaithProcessionsLawHandler : ILawHandler
    {
        readonly FaithProcessionsLaw _law;
        readonly IPopupService _popup;

        const double MoraleThreshold = 40;
        const double ImmediateMorale = 15;
        const double ImmediateMaterials = -10;
        const double ImmediateUnrest = 5;
        const double DailyMorale = 2;
        const double DailySickness = 1;

        public FaithProcessionsLawHandler(FaithProcessionsLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => state.Morale < MoraleThreshold;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Faith Processions");

            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Faith Processions");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Faith Processions");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Faith Processions");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Faith Processions");
        }
    }
}
