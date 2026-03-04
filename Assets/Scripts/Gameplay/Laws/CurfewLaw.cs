using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CurfewLaw : Law
    {
        const double UnrestThreshold = 50;
        const double DailyUnrest = -5;

        public override string Id => "curfew";
        public override string Name => "Curfew";
        public override string Description => "Enforce a nightly curfew. Reduces unrest but slows production.";
        public override string NarrativeText => "After dark, only guards walk the streets.";

        public override double ProductionMultiplier => 0.85;

        public override bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("martial_law");

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Curfew");
        }
    }
}
