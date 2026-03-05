using AutofacUnity;
using Siege.Gameplay.Resources;
using UnityEngine;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Component on buildings that store resources. Owns a ResourceInventory and registers it
    /// with ResourceLedger when active. All storage logic lives in ResourceInventory.
    /// </summary>
    public class StorageBuilding : MonoBehaviour
    {
        public const double DefaultMaxCapacityPerResource = 200;

        [SerializeField] double _maxCapacityPerResource = DefaultMaxCapacityPerResource;

        ResourceLedger _ledger;

        public ResourceInventory Inventory { get; private set; }
        public Building Building { get; private set; }

        // ── Lifecycle ─────────────────────────────────────────────────

        void Awake()
        {
            Building = GetComponent<Building>();
            Inventory = new ResourceInventory { MaxPerResource = _maxCapacityPerResource };
            _ledger = Resolver.Resolve<ResourceLedger>();
        }

        void OnEnable()
        {
            if (Building?.Zone != null)
                _ledger?.Register(Inventory, Building.Zone);
        }

        void OnDisable() => _ledger?.Unregister(Inventory);

        // ── Convenience Accessors (delegate to Inventory) ─────────────

        public double GetStored(ResourceType type) => Inventory.GetStored(type);
        public double Deposit(ResourceType type, double amount) => Inventory.Deposit(type, amount);
        public double Withdraw(ResourceType type, double amount) => Inventory.Withdraw(type, amount);
        public void ClearAll() => Inventory.ClearAll();

        public System.Collections.Generic.Dictionary<ResourceType, double> GetAllStored() =>
            new(Inventory.GetSnapshot());
    }
}
