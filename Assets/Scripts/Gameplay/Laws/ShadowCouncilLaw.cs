using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ShadowCouncilLaw : ILaw
    {
        const string Narrative = "No one knows who sits on the council. That is the point.";

        readonly IPopupService _popup;
        readonly PoliticalState _politics;

        public ShadowCouncilLaw(IPopupService popup, PoliticalState politics)
        {
            _popup = popup;
            _politics = politics;
        }

        public string Id => "shadow_council";
        public string Name => "Shadow Council";
        public string Description => "Cede power to a secretive inner circle. They keep order through disappearances.";

        public bool CanEnact(GameState state) => _politics.Tyranny.Value >= 5;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;

            state.ProductionMultiplier *= 1.05;
            log.Record("ProductionMultiplier", 0.05, Name);

            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest -= 3;
            log.Record("Unrest", -3, Name);

            if (state.HealthyWorkers > 0)
            {
                state.HealthyWorkers -= 1;
                state.DeathsToday += 1;
                state.TotalDeaths += 1;
                log.Record("HealthyWorkers", -1, Name);
                log.Record("Deaths", 1, Name);
            }

            if (state.Morale > 30)
            {
                var delta = state.Morale - 30;
                state.Morale = 30;
                log.Record("Morale", -delta, Name);
            }
        }

        public ILaw Clone() => new ShadowCouncilLaw(_popup, _politics);
    }
}
