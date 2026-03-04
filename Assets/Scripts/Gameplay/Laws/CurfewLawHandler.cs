using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CurfewLawHandler : ILawHandler
    {
        readonly CurfewLaw _law;
        readonly IPopupService _popup;

        const double UnrestThreshold = 50;
        const double DailyUnrest = -5;

        public CurfewLawHandler(CurfewLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("martial_law");

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Curfew");
        }
    }
}
