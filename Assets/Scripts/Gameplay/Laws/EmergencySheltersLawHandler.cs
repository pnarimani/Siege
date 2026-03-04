using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class EmergencySheltersLawHandler : ILawHandler
    {
        readonly EmergencySheltersLaw _law;
        readonly IPopupService _popup;

        const int CapacityBonus = 4;
        const double ImmediateUnrest = 8;
        const double DailySickness = 2;
        const double DailyUnrest = 2;

        public EmergencySheltersLawHandler(EmergencySheltersLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state)
        {
            foreach (var zone in state.Zones.Values)
                if (zone.IsLost) return true;
            return false;
        }

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            foreach (var zone in state.Zones.Values)
            {
                if (zone.IsLost) continue;
                zone.Capacity += CapacityBonus;
                log.Record("ZoneCapacity", CapacityBonus, $"Emergency Shelters ({zone.Id})");
            }

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Emergency Shelters");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Emergency Shelters");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Emergency Shelters");
        }
    }
}
