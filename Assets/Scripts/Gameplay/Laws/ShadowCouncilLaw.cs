using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ShadowCouncilLaw : Law
    {
        const double DailyUnrest = -3;
        const int DailyDeaths = 1;
        const double MoraleCap = 30;

        public override string Id => "shadow_council";
        public override string Name => "Shadow Council";
        public override string Description => "Cede power to a secretive inner circle. They keep order through disappearances.";
        public override string NarrativeText => "No one knows who sits on the council. That is the point.";

        public override double ProductionMultiplier => 1.05;

        public override bool CanEnact(GameState state) =>
            Resolver.Resolve<PoliticalState>().Tyranny.Value >= 5;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Shadow Council");

            if (state.HealthyWorkers > 0)
            {
                state.HealthyWorkers -= DailyDeaths;
                state.TotalDeaths += DailyDeaths;
                state.DeathsToday += DailyDeaths;
                log.Record("HealthyWorkers", -DailyDeaths, "Shadow Council (disappearance)");
            }

            if (state.Morale > MoraleCap)
            {
                double reduction = state.Morale - MoraleCap;
                state.Morale = MoraleCap;
                log.Record("Morale", -reduction, "Shadow Council (cap)");
            }
        }
    }
}
