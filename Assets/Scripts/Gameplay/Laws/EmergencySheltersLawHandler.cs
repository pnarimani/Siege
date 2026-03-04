using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class EmergencySheltersLawHandler : LawHandler<EmergencySheltersLaw>
    {
        const int CapacityBonus = 4;
        const double ImmediateUnrest = 8;
        const double DailySickness = 2;
        const double DailyUnrest = 2;

        public EmergencySheltersLawHandler(EmergencySheltersLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state)
        {
            foreach (var zone in state.Zones.Values)
                if (zone.IsLost) return true;
            return false;
        }

        public override void ApplyImmediate(GameState state, ChangeLog log)
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
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Emergency Shelters");

            state.Unrest += DailyUnrest;
            log.Record("Unrest", DailyUnrest, "Emergency Shelters");
        }
    }
}
