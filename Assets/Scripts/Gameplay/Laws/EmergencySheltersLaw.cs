using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class EmergencySheltersLaw : Law
    {
        const int CapacityBonus = 4;
        const double ImmediateUnrest = 8;
        const double DailySickness = 2;
        const double DailyUnrest = 2;

        public override string Id => "emergency_shelters";
        public override string Name => "Emergency Shelters";
        public override string Description => "Erect makeshift shelters in surviving zones. Increases capacity but worsens overcrowding disease.";
        public override string NarrativeText => "Canvas and rope. It is not a home, but it keeps the rain off the children.";

        public override bool CanEnact(GameState state)
        {
            foreach (var zone in state.Zones.Values)
                if (zone.IsLost) return true;
            return false;
        }

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            foreach (var zone in state.Zones.Values)
            {
                if (zone.IsLost) continue;
                zone.Capacity += CapacityBonus;
                log.Record("ZoneCapacity", CapacityBonus, $"Emergency Shelters ({zone.Id})");
            }

            state.Unrest += ImmediateUnrest;
            log.Record("Unrest", ImmediateUnrest, "Emergency Shelters");
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
