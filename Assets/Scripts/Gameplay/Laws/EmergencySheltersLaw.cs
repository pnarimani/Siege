using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class EmergencySheltersLaw : ILaw
    {
        readonly IPopupService _popup;

        const string Narrative = "Canvas and rope. It is not a home, but it keeps the rain off the children.";
        const int CapacityBonus = 4;
        const double ImmediateUnrest = 8;
        const double DailySickness = 2;
        const double DailyUnrest = 2;

        public EmergencySheltersLaw(IPopupService popup) => _popup = popup;

        public string Id => "emergency_shelters";
        public string Name => "Emergency Shelters";
        public string Description => "Erect makeshift shelters in surviving zones. Increases capacity but worsens overcrowding disease.";

        public bool CanEnact(GameState state)
        {
            foreach (var zone in state.Zones.Values)
                if (zone.IsLost) return true;
            return false;
        }

        public void OnEnact(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Emergency Shelters");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Emergency Shelters");
        }

        public ILaw Clone() => new EmergencySheltersLaw(_popup);
    }
}
