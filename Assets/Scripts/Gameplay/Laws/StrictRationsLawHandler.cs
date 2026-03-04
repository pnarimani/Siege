using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class StrictRationsLawHandler : LawHandler<StrictRationsLaw>
    {
        const double ImmediateMorale = -10;
        const double DailyUnrest = 3;
        const double DailySickness = 1;

        public StrictRationsLawHandler(StrictRationsLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => true;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Strict Rations");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Strict Rations");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Strict Rations");
        }
    }
}
