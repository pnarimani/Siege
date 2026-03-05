using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class OathOfMercyLaw : ILaw
    {
        const string Narrative = "We swore to protect them. All of them. Even when it costs us.";

        readonly IPopupService _popup;

        public OathOfMercyLaw(IPopupService popup) => _popup = popup;

        public string Id => "oath_of_mercy";
        public string Name => "Oath of Mercy";
        public string Description => "Swear a public oath to protect all citizens. Lifts morale and eases sickness, but slows output.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;
            state.ProductionMultiplier *= 0.9;
            log.Record("ProductionMultiplier", -0.1, Name);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Morale += 5;
            log.Record("Morale", 5, Name);

            state.Sickness -= 2;
            log.Record("Sickness", -2, Name);
        }

        public ILaw Clone() => new OathOfMercyLaw(_popup);
    }
}
