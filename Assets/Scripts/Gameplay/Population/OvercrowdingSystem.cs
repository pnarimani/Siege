using Siege.Gameplay.Simulation;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Population
{
    /// <summary>
    /// When zones are overcrowded (population > capacity), apply stacking penalties:
    /// increased unrest, sickness, and food consumption.
    /// </summary>
    public class OvercrowdingSystem : ISimulationSystem
    {
        const double UnrestPerOvercrowdedPerson = 0.3;
        const double SicknessPerOvercrowdedPerson = 0.2;
        const double MoralePenaltyPerOvercrowded = 0.1;

        readonly ChangeLog _changeLog;

        public OvercrowdingSystem(ChangeLog changeLog)
        {
            _changeLog = changeLog;
        }

        public void Tick(GameState state, float deltaTime)
        {
            float dayFraction = deltaTime / GameClock.DayLengthSeconds;

            foreach (var zone in state.Zones.Values)
            {
                if (zone.IsLost) continue;
                int excess = zone.Population - zone.Capacity;
                if (excess <= 0) continue;

                double unrest = excess * UnrestPerOvercrowdedPerson * dayFraction;
                double sickness = excess * SicknessPerOvercrowdedPerson * dayFraction;
                double morale = excess * MoralePenaltyPerOvercrowded * dayFraction;

                state.Unrest += unrest;
                state.Sickness += sickness;
                state.Morale -= morale;

                _changeLog.Record("Unrest", unrest, $"Overcrowding ({zone.Id})");
                _changeLog.Record("Sickness", sickness, $"Overcrowding ({zone.Id})");
                _changeLog.Record("Morale", -morale, $"Overcrowding ({zone.Id})");
            }
        }
    }
}
