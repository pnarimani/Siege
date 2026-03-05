using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class PurgeTheDisloyalLaw : ILaw
    {
        const string Narrative = "The lists were drawn up at midnight. By dawn, eight cells were empty.";

        readonly IPopupService _popup;
        readonly PoliticalState _politics;

        public PurgeTheDisloyalLaw(IPopupService popup, PoliticalState politics)
        {
            _popup = popup;
            _politics = politics;
        }

        public string Id => "purge_disloyal";
        public string Name => "Purge the Disloyal";
        public string Description => "Root out suspected traitors and make examples of them. Devastating but effective.";

        public bool CanEnact(GameState state) => _politics.Tyranny.Value >= 6;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;

            state.Unrest -= 30;
            log.Record("Unrest", -30, Name);

            state.Morale -= 15;
            log.Record("Morale", -15, Name);

            state.HealthyWorkers -= 8;
            state.DeathsToday += 8;
            state.TotalDeaths += 8;
            log.Record("HealthyWorkers", -8, Name);
            log.Record("Deaths", 8, Name);

            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public ILaw Clone() => new PurgeTheDisloyalLaw(_popup, _politics);
    }
}
