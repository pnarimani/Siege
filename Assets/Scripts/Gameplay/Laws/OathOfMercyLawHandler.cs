using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class OathOfMercyLawHandler : ILawHandler
    {
        readonly OathOfMercyLaw _law;
        readonly IPopupService _popup;

        const double DailyMorale = 5;
        const double DailySickness = -2;

        public OathOfMercyLawHandler(OathOfMercyLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => true;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Oath of Mercy");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Oath of Mercy");
        }
    }
}
