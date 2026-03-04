using Siege.Gameplay.Buildings;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Resources
{
    /// <summary>
    /// Ticks all active buildings: subtracts inputs, adds outputs, scaled by assigned workers and day length.
    /// Production rates in BuildingDefinition are per-worker-per-day.
    /// </summary>
    public class ResourceProductionSystem : ISimulationSystem
    {
        readonly ChangeLog _changeLog;
        readonly ResourceStorage _storage;
        readonly LawDispatcher _lawDispatcher;

        public ResourceProductionSystem(ChangeLog changeLog, ResourceStorage storage, LawDispatcher lawDispatcher)
        {
            _changeLog = changeLog;
            _storage = storage;
            _lawDispatcher = lawDispatcher;
        }

        public void Tick(GameState state, float deltaTime)
        {
            float dayFraction = deltaTime / GameClock.DayLengthSeconds;

            foreach (var building in Building.All)
            {
                if (!building.IsActive || building.NeedsRepair) continue;
                if (building.Zone != null && building.Zone.IsLost) continue;
                if (building.AssignedWorkers <= 0) continue;
                if (building.GetComponent<ProductionCycleState>() != null) continue; // handled by real-time cycle

                int workers = building.AssignedWorkers;
                var inputs = building.GetCurrentInputs();
                var outputs = building.GetCurrentOutputs();

                // Check if all inputs are available
                bool canProduce = true;
                foreach (var input in inputs)
                {
                    if (input.Resource == ResourceType.Integrity || input.Resource == ResourceType.Care)
                        continue;

                    double needed = input.Quantity * workers * dayFraction;
                    double available = _storage.GetTotal(input.Resource);
                    if (available < needed)
                    {
                        canProduce = false;
                        break;
                    }
                }

                if (!canProduce) continue;

                // Consume inputs
                foreach (var input in inputs)
                {
                    if (input.Resource == ResourceType.Integrity || input.Resource == ResourceType.Care)
                        continue;

                    double amount = input.Quantity * workers * dayFraction;
                    _storage.Withdraw(input.Resource, amount);
                    state.AddResource(input.Resource, -amount);
                    _changeLog.Record(input.Resource.ToString(), -amount, building.Definition.Name);
                }

                // Produce outputs
                foreach (var output in outputs)
                {
                    double amount = output.Quantity * workers * dayFraction * _lawDispatcher.CombinedProductionMultiplier;

                    if (output.Resource == ResourceType.Integrity)
                    {
                        // Integrity goes to the building's zone
                        if (building.Zone != null)
                        {
                            var zoneState = state.Zones[building.Zone.Id];
                            zoneState.Integrity = System.Math.Min(100, zoneState.Integrity + amount);
                            _changeLog.Record("Integrity", amount, building.Definition.Name);
                        }
                        continue;
                    }

                    if (output.Resource == ResourceType.Care)
                    {
                        // Care is handled by CareSystem, just track production
                        state.AddResource(ResourceType.Care, amount);
                        _changeLog.Record("Care", amount, building.Definition.Name);
                        continue;
                    }

                    _storage.Deposit(output.Resource, amount);
                    state.AddResource(output.Resource, amount);
                    _changeLog.Record(output.Resource.ToString(), amount, building.Definition.Name);
                }
            }
        }
    }
}
