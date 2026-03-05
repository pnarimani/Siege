using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CollectiveFarmsLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "The fields belong to everyone now. Not everyone agrees.";
        const double ImmediateMorale = 5;
        const double DailyUnrest = 3;

        public CollectiveFarmsLaw(IPopupService popup) => _popup = popup;

        public string Id => "collective_farms";
        public string Name => "Collective Farms";
        public string Description => "Communalize all food production. Boosts output but breeds resentment among former landowners.";

        public bool CanEnact(GameState state) => true;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.ProductionMultiplier *= 1.3;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Collective Farms");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Collective Farms");
        }

        public ILaw Clone() => new CollectiveFarmsLaw(_popup);
    }
}
