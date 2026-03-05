using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class StrictRationsLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "Half a bowl. That is what each person gets. Half a bowl, and silence.";
        const double ImmediateMorale = -10;
        const double DailyUnrest = 3;
        const double DailySickness = 1;

        public StrictRationsLaw(IPopupService popup) => _popup = popup;

        public string Id => "strict_rations";
        public string Name => "Strict Rations";
        public string Description => "Cut food portions to stretch supplies. Hunger gnaws, but stores last longer.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.FoodConsumptionMultiplier *= 0.75;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Strict Rations");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Strict Rations");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Strict Rations");
        }

        public ILaw Clone() => new StrictRationsLaw(_popup);
    }
}
