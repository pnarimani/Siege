using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using Siege.Gameplay;

namespace Siege.Gameplay.Laws
{
    public class AbandonOuterRingLawHandler : LawHandler<AbandonOuterRingLaw>
    {
        const double IntegrityThreshold = 40;
        const double UnrestIncrease = 15;

        public AbandonOuterRingLawHandler(AbandonOuterRingLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            return !zone.IsLost && zone.Integrity < IntegrityThreshold;
        }

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("Zone", 0, "OuterFarms lost (Abandon Outer Ring)");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Abandon Outer Ring");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }
    }
}
