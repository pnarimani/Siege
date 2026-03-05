using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CurfewLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "After dark, only guards walk the streets.";
        const double UnrestThreshold = 50;
        const double DailyUnrest = -5;

        public CurfewLaw(IPopupService popup) => _popup = popup;

        public string Id => "curfew";
        public string Name => "Curfew";
        public string Description => "Enforce a nightly curfew. Reduces unrest but slows production.";

        public bool CanEnact(GameState state) =>
            state.Unrest > UnrestThreshold && !state.EnactedLawIds.Contains("martial_law");

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.ProductionMultiplier *= 0.85;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Curfew");
        }

        public ILaw Clone() => new CurfewLaw(_popup);
    }
}
