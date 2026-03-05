using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class FaithProcessionsLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "They sing as they walk. For a moment, the walls do not feel so close.";
        const double MoraleThreshold = 40;
        const double ImmediateMorale = 15;
        const double ImmediateMaterials = -10;
        const double ImmediateUnrest = 5;
        const double DailyMorale = 2;
        const double DailySickness = 1;

        public FaithProcessionsLaw(IPopupService popup) => _popup = popup;

        public string Id => "faith_processions";
        public string Name => "Faith Processions";
        public string Description => "Organize daily religious processions through the streets. Lifts spirits but risks spreading disease.";

        public bool CanEnact(GameState state) => state.Morale < MoraleThreshold;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Faith Processions");

            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Faith Processions");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Faith Processions");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Faith Processions");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Faith Processions");
        }

        public ILaw Clone() => new FaithProcessionsLaw(_popup);
    }
}
