using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Buildings
{
    public class BuildingService
    {
        readonly StorageBuildingRegistry _storageBuildings;
        readonly ResourceStorage _resourceStorage;
        readonly GameState _gameState;

        public BuildingService(StorageBuildingRegistry storageBuildings, ResourceStorage resourceStorage, GameState gameState)
        {
            _storageBuildings = storageBuildings;
            _resourceStorage = resourceStorage;
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
            var stored = storage.GetAllStored();
            storage.ClearAll();

            foreach (var (resource, amount) in stored)
            {
                double deposited = 0;
                foreach (var other in _storageBuildings.All)
                {
                    if (other == storage) continue;
                    deposited += other.Deposit(resource, amount - deposited);
                    if (deposited >= amount) break;
                }

                var lost = amount - deposited;
                if (lost > 0)
                    _gameState.AddResource(resource, -lost);
            }
        }

        void GrantSalvageMaterials(Building building)
        {
            foreach (var salvage in building.Definition.SalvageMaterials)
            {
                _resourceStorage.Deposit(salvage.Resource, salvage.Quantity);
                _gameState.AddResource(salvage.Resource, salvage.Quantity);
            }
        }
    }
}
