using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using Siege.Gameplay;

namespace Siege.Gameplay.Laws
{
    public class AbandonOuterRingLawHandler : ILawHandler
    {
        readonly AbandonOuterRingLaw _law;
        readonly IPopupService _popup;

        const double IntegrityThreshold = 40;
        const double UnrestIncrease = 15;

        public AbandonOuterRingLawHandler(AbandonOuterRingLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            return !zone.IsLost && zone.Integrity < IntegrityThreshold;
        }

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("Zone", 0, "OuterFarms lost (Abandon Outer Ring)");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Abandon Outer Ring");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }
    }
}
