using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLaw : Law
    {
        const double ImmediateMaterials = -20;
        const double ImmediateUnrest = 5;
        const double DailyUnrest = 5;

        public override string Id => "scorched_earth";
        public override string Name => "Scorched Earth";
        public override string Description => "Burn everything the enemy might use. Reduces siege damage but destroys materials and breeds unrest.";
        public override string NarrativeText => "If we cannot hold it, neither shall they. Light the fires.";

        public override double SiegeDamageMultiplier => 0.7;

        public override bool CanEnact(GameState state) =>
            Resolver.Resolve<PoliticalState>().Fortification.Value >= 3;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += ImmediateMaterials;
            log.Record("Materials", ImmediateMaterials, "Scorched Earth");

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Scorched Earth");
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Scorched Earth");
        }
    }
}
