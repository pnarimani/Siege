using System;
using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Buildings;
using UnityEngine;

namespace Siege.Gameplay.Zones
{
    /// <summary>
    /// A physical zone in the 3D world. Contains buildings and tracks integrity/capacity.
    /// ZoneId is derived from the GameObject name (must match enum name).
    /// </summary>
    public class Zone : MonoBehaviour
    {
        ZoneRegistry _registry;

        // ── Runtime State ─────────────────────────────────────────────
        public ZoneId Id { get; private set; }
        public bool IsLost { get; set; }

        List<Building> _buildings = new();
        public IReadOnlyList<Building> Buildings => _buildings;

        // ── Lifecycle ─────────────────────────────────────────────────

        void OnEnable() => _registry?.Register(this);
        void OnDisable() => _registry?.Unregister(this);

        void Awake()
        {
            _registry = Resolver.Resolve<ZoneRegistry>();
            Id = Enum.Parse<ZoneId>(gameObject.name);
        }

        /// <summary>
        /// Call after all Building children have been added to rebuild the building list.
        /// </summary>
        public void RefreshBuildings()
        {
            _buildings = new List<Building>(GetComponentsInChildren<Building>());
        }

        /// <summary>
        /// Returns all storage buildings in this zone.
        /// </summary>
        public List<StorageBuilding> GetStorageBuildings()
        {
            var result = new List<StorageBuilding>();
            foreach (var b in _buildings)
            {
                var storage = b.GetComponent<StorageBuilding>();
                if (storage != null) result.Add(storage);
            }
            return result;
        }

        /// <summary>
        /// Deactivate all buildings when zone is lost.
        /// </summary>
        public void OnZoneLost()
        {
            IsLost = true;
            foreach (var b in _buildings)
            {
                b.IsActive = false;
                b.AssignedWorkers = 0;
            }
        }
    }
}
