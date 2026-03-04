using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CollectiveFarmsLaw : Law
    {
        const double ImmediateMorale = 5;
        const double DailyUnrest = 3;

        public override string Id => "collective_farms";
        public override string Name => "Collective Farms";
        public override string Description => "Communalize all food production. Boosts output but breeds resentment among former landowners.";
        public override string NarrativeText => "The fields belong to everyone now. Not everyone agrees.";

        public override double ProductionMultiplier => 1.3;

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Collective Farms");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Collective Farms");
        }
    }
}
