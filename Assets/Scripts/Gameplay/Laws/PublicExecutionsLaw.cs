using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class PublicExecutionsLaw : ILaw
    {
        const string Narrative = "The crowd watches in silence. Fear is a kind of obedience.";

        readonly IPopupService _popup;

        public PublicExecutionsLaw(IPopupService popup) => _popup = popup;

        public string Id => "public_executions";
        public string Name => "Public Executions";
        public string Description => "Execute dissidents publicly to restore order. Crushes unrest but shatters morale.";

        public bool CanEnact(GameState state) => state.Unrest > 60;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;

            state.Unrest -= 25;
            log.Record("Unrest", -25, Name);

            state.Morale -= 20;
            log.Record("Morale", -20, Name);

            state.HealthyWorkers -= 5;
            state.DeathsToday += 5;
            state.TotalDeaths += 5;
            log.Record("HealthyWorkers", -5, Name);
            log.Record("Deaths", 5, Name);

            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public ILaw Clone() => new PublicExecutionsLaw(_popup);
    }
}
