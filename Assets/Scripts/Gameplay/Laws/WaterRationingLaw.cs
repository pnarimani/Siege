using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class WaterRationingLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "The wells are guarded now. One cup per person. No exceptions.";
        const double ImmediateMorale = -10;
        const double DailySickness = 1;
        const double DailyUnrest = 2;

        public WaterRationingLaw(IPopupService popup) => _popup = popup;

        public string Id => "water_rationing";
        public string Name => "Water Rationing";
        public string Description => "Restrict water usage to essential needs only. Preserves reserves but breeds disease and discontent.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.WaterConsumptionMultiplier *= 0.75;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Water Rationing");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Water Rationing");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Water Rationing");
        }

        public ILaw Clone() => new WaterRationingLaw(_popup);
    }
}
