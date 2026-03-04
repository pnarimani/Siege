using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class OathOfMercyLawHandler : LawHandler<OathOfMercyLaw>
    {
        const double DailyMorale = 5;
        const double DailySickness = -2;

        public OathOfMercyLawHandler(OathOfMercyLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => true;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Oath of Mercy");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Oath of Mercy");
        }
    }
}
