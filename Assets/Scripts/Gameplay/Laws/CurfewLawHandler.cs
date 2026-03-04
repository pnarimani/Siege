using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CurfewLawHandler : LawHandler<CurfewLaw>
    {
        const double UnrestThreshold = 50;
        const double DailyUnrest = -5;

        public CurfewLawHandler(CurfewLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("martial_law");

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Curfew");
        }
    }
}
