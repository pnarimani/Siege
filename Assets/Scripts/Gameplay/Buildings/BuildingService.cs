using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Buildings
{
    public class BuildingService
    {
        readonly ResourceLedger _ledger;
        readonly GameState _gameState;

        public BuildingService(ResourceLedger ledger, GameState gameState)
        {
            _ledger = ledger;
            _gameState = gameState;
        }

        /// <summary>
        /// Destroys a building: redistributes stored resources, grants salvage materials, then destroys the GameObject.
        /// </summary>
        public void DestroyBuilding(Building building)
        {
            var storageBuilding = building.GetComponent<StorageBuilding>();
            if (storageBuilding != null)
                RedistributeStorage(storageBuilding);

            GrantSalvageMaterials(building);
            UnityEngine.Object.Destroy(building.gameObject);
        }

        void RedistributeStorage(StorageBuilding storage)
        {
            var snapshot = storage.Inventory.GetSnapshot();

            // Unregister first so redistributed resources don't flow back into this inventory
            _ledger.Unregister(storage.Inventory);

            foreach (var (resource, amount) in snapshot)
                _ledger.Deposit(resource, amount);

            storage.ClearAll();
        }

        void GrantSalvageMaterials(Building building)
        {
            foreach (var salvage in building.Definition.SalvageMaterials)
                _ledger.Deposit(salvage.Resource, salvage.Quantity);
        }
    }
}
