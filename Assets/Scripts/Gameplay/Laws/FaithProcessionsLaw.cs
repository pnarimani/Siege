using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class FaithProcessionsLaw : Law
    {
        const double MoraleThreshold = 40;
        const double ImmediateMorale = 15;
        const double ImmediateMaterials = -10;
        const double ImmediateUnrest = 5;
        const double DailyMorale = 2;
        const double DailySickness = 1;

        public override string Id => "faith_processions";
        public override string Name => "Faith Processions";
        public override string Description => "Organize daily religious processions through the streets. Lifts spirits but risks spreading disease.";
        public override string NarrativeText => "They sing as they walk. For a moment, the walls do not feel so close.";

        public override bool CanEnact(GameState state) => state.Morale < MoraleThreshold;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Faith Processions");

            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Faith Processions");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Faith Processions");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Faith Processions");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Faith Processions");
        }
    }
}
