using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class WaterRationingLawHandler : LawHandler<WaterRationingLaw>
    {
        const double ImmediateMorale = -10;
        const double DailySickness = 1;
        const double DailyUnrest = 2;

        public WaterRationingLawHandler(WaterRationingLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => true;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Water Rationing");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Water Rationing");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Water Rationing");
        }
    }
}
