using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using Siege.Gameplay;

namespace Siege.Gameplay.Laws
{
    public class AbandonOuterRingLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "The outer fields are lost. Pull everyone back behind the second wall \u2014 and pray it holds.";
        const double IntegrityThreshold = 40;
        const double UnrestIncrease = 15;

        public AbandonOuterRingLaw(IPopupService popup) => _popup = popup;

        public string Id => "abandon_outer_ring";
        public string Name => "Abandon the Outer Ring";
        public string Description => "Withdraw all forces from the outer farms, ceding them to the enemy. Reduces siege pressure but causes unrest.";

        public bool CanEnact(GameState state)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            return !zone.IsLost && zone.Integrity < IntegrityThreshold;
        }

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.SiegeDamageMultiplier *= 0.8;

            var zone = state.Zones[ZoneId.OuterFarms];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("Zone", 0, "OuterFarms lost (Abandon Outer Ring)");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Abandon Outer Ring");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public ILaw Clone() => new AbandonOuterRingLaw(_popup);
    }
}
