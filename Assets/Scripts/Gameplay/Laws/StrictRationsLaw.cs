using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class StrictRationsLaw : Law
    {
        const double ImmediateMorale = -10;
        const double DailyUnrest = 3;
        const double DailySickness = 1;

        public override string Id => "strict_rations";
        public override string Name => "Strict Rations";
        public override string Description => "Cut food portions to stretch supplies. Hunger gnaws, but stores last longer.";
        public override string NarrativeText => "Half a bowl. That is what each person gets. Half a bowl, and silence.";

        public override double FoodConsumptionMultiplier => 0.75;

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Strict Rations");
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Strict Rations");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Strict Rations");
        }
    }
}
