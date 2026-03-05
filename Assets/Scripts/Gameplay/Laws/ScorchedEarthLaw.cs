using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class ScorchedEarthLaw : ILaw
    {
        const string Narrative = "If we cannot hold it, neither shall they. Light the fires.";

        readonly IPopupService _popup;
        readonly PoliticalState _politics;

        public ScorchedEarthLaw(IPopupService popup, PoliticalState politics)
        {
            _popup = popup;
            _politics = politics;
        }

        public string Id => "scorched_earth";
        public string Name => "Scorched Earth";
        public string Description => "Burn everything the enemy might use. Reduces siege damage but destroys materials and breeds unrest.";

        public bool CanEnact(GameState state) => _politics.Fortification.Value >= 3;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;

            state.Materials -= 20;
            log.Record("Materials", -20, Name);

            state.Unrest += 5;
            log.Record("Unrest", 5, Name);

            state.SiegeDamageMultiplier *= 0.7;
            log.Record("SiegeDamageMultiplier", -0.3, Name);

            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest += 5;
            log.Record("Unrest", 5, Name);
        }

        public ILaw Clone() => new ScorchedEarthLaw(_popup, _politics);
    }
}
