using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CollectiveFarmsLawHandler : LawHandler<CollectiveFarmsLaw>
    {
        const double ImmediateMorale = 5;
        const double DailyUnrest = 3;

        public CollectiveFarmsLawHandler(CollectiveFarmsLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => true;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Collective Farms");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Collective Farms");
        }
    }
}
