using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ShadowCouncilLawHandler : LawHandler<ShadowCouncilLaw>
    {
        const double DailyUnrest = -3;
        const int DailyDeaths = 1;
        const double MoraleCap = 30;

        readonly PoliticalState _political;

        public ShadowCouncilLawHandler(ShadowCouncilLaw law, IPopupService popup, PoliticalState political) : base(law, popup)
        {
            _political = political;
        }

        public override bool CanEnact(GameState state) =>
            _political.Tyranny.Value >= 5;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
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
