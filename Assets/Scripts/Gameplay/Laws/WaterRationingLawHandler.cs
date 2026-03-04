using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class WaterRationingLawHandler : ILawHandler
    {
        readonly WaterRationingLaw _law;
        readonly IPopupService _popup;

        const double ImmediateMorale = -10;
        const double DailySickness = 1;
        const double DailyUnrest = 2;

        public WaterRationingLawHandler(WaterRationingLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => true;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Water Rationing");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Water Rationing");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Water Rationing");
        }
    }
}
