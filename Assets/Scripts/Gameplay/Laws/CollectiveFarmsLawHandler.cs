using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class CollectiveFarmsLawHandler : ILawHandler
    {
        readonly CollectiveFarmsLaw _law;
        readonly IPopupService _popup;

        const double ImmediateMorale = 5;
        const double DailyUnrest = 3;

        public CollectiveFarmsLawHandler(CollectiveFarmsLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => true;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += ImmediateMorale;
            log.Record("Morale", ImmediateMorale, "Collective Farms");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Collective Farms");
        }
    }
}
