using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class WaterRationingLaw : Law
    {
        const double ImmediateMorale = -10;
        const double DailySickness = 1;
        const double DailyUnrest = 2;

        public override string Id => "water_rationing";
        public override string Name => "Water Rationing";
        public override string Description => "Restrict water usage to essential needs only. Preserves reserves but breeds disease and discontent.";
        public override string NarrativeText => "The wells are guarded now. One cup per person. No exceptions.";

        public override double WaterConsumptionMultiplier => 0.75;

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Water Rationing");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Water Rationing");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Water Rationing");
        }
    }
}
