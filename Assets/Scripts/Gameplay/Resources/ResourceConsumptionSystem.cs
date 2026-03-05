using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Resources
{
    /// <summary>
    /// Population consumes food and water each day. Rates are per-person-per-day.
    /// Prioritizes withdrawal from perimeter zone storage first, then inward.
    /// </summary>
    public class ResourceConsumptionSystem : ISimulationSystem
    {
        // Per-person-per-day consumption
        const double FoodPerPerson = 1.5;
        const double WaterPerPerson = 1.2;
        const double FuelPerPersonNight = 0.3; // only consumed at night for warmth

        readonly ChangeLog _changeLog;
        readonly ResourceLedger _ledger;
        readonly GameClock _clock;

        public ResourceConsumptionSystem(ChangeLog changeLog, ResourceLedger ledger, GameClock clock)
        {
            _changeLog = changeLog;
            _ledger = ledger;
            _clock = clock;
        }

        public void Tick(GameState state, float deltaTime)
        {
            float dayFraction = deltaTime / GameClock.DayLengthSeconds;
            int consumers = state.TotalConsumers;
            if (consumers <= 0) return;

            // Food consumption
            double foodNeeded = FoodPerPerson * consumers * dayFraction * state.FoodConsumptionMultiplier;
            double foodConsumed = _ledger.Withdraw(ResourceType.Food, foodNeeded);
            if (foodConsumed > 0)
                _changeLog.Record("Food", -foodConsumed, "Population consumption");

            // Water consumption
            double waterNeeded = WaterPerPerson * consumers * dayFraction * state.WaterConsumptionMultiplier;
            double waterConsumed = _ledger.Withdraw(ResourceType.Water, waterNeeded);
            if (waterConsumed > 0)
                _changeLog.Record("Water", -waterConsumed, "Population consumption");

            // Fuel consumption at night
            if (_clock.IsNight)
            {
                double fuelNeeded = FuelPerPersonNight * consumers * dayFraction;
                double fuelConsumed = _ledger.Withdraw(ResourceType.Fuel, fuelNeeded);
                if (fuelConsumed > 0)
                    _changeLog.Record("Fuel", -fuelConsumed, "Night warmth");

                // No fuel at night → morale hit
                if (fuelConsumed < fuelNeeded * 0.5)
                {
                    double moralePenalty = 2.0 * dayFraction;
                    state.Morale -= moralePenalty;
                    _changeLog.Record("Morale", -moralePenalty, "No fuel for warmth");
                }
            }

            // Hunger/thirst morale effects
            if (_ledger.GetTotal(ResourceType.Food) <= 0)
            {
                double penalty = 5.0 * dayFraction;
                state.Morale -= penalty;
                state.Unrest += penalty * 0.5;
                _changeLog.Record("Morale", -penalty, "Starvation");
                _changeLog.Record("Unrest", penalty * 0.5, "Starvation");
            }

            if (_ledger.GetTotal(ResourceType.Water) <= 0)
            {
                double penalty = 6.0 * dayFraction;
                state.Morale -= penalty;
                state.Sickness += penalty * 0.3;
                _changeLog.Record("Morale", -penalty, "Dehydration");
                _changeLog.Record("Sickness", penalty * 0.3, "Dehydration");
            }
        }
    }
}
