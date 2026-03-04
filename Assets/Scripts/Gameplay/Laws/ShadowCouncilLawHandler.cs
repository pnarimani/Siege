using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ShadowCouncilLawHandler : ILawHandler
    {
        readonly ShadowCouncilLaw _law;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const double DailyUnrest = -3;
        const int DailyDeaths = 1;
        const double MoraleCap = 30;

        public ShadowCouncilLawHandler(ShadowCouncilLaw law, IPopupService popup, PoliticalState political)
        {
            _law = law;
            _popup = popup;
            _political = political;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) =>
            _political.Tyranny.Value >= 5;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
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
