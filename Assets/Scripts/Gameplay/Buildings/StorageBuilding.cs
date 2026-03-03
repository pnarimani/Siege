using System.Collections.Generic;
using UnityEngine;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Component on buildings that store resources. Resources are physically located
    /// in storage buildings. Losing a zone without evacuating loses stored resources.
    /// </summary>
    public class StorageBuilding : MonoBehaviour
    {
        // ── Static Registry ───────────────────────────────────────────
        static readonly List<StorageBuilding> _all = new();
        public static IReadOnlyList<StorageBuilding> All => _all;

        // ── Configuration ─────────────────────────────────────────────
        [SerializeField] double _maxCapacityPerResource = 200;

        // ── State ─────────────────────────────────────────────────────
        readonly Dictionary<ResourceType, double> _stored = new();
        public Building Building { get; private set; }

        public double MaxCapacityPerResource => _maxCapacityPerResource;

        // ── Lifecycle ─────────────────────────────────────────────────

        void OnEnable() => _all.Add(this);
        void OnDisable() => _all.Remove(this);

        void Awake()
        {
            Building = GetComponent<Building>();
        }

        // ── Storage Operations ────────────────────────────────────────

        public double GetStored(ResourceType type)
        {
            return _stored.TryGetValue(type, out var amount) ? amount : 0;
        }

        /// <summary>
        /// Deposit resources. Returns the amount actually deposited (may be less if at capacity).
        /// </summary>
        public double Deposit(ResourceType type, double amount)
        {
            double current = GetStored(type);
            double space = _maxCapacityPerResource - current;
            double deposited = System.Math.Min(amount, space);
            if (deposited > 0)
                _stored[type] = current + deposited;
            return deposited;
        }

        /// <summary>
        /// Withdraw resources. Returns the amount actually withdrawn (may be less if insufficient).
        /// </summary>
        public double Withdraw(ResourceType type, double amount)
        {
            double current = GetStored(type);
            double withdrawn = System.Math.Min(amount, current);
            if (withdrawn > 0)
                _stored[type] = current - withdrawn;
            return withdrawn;
        }

        /// <summary>
        /// Returns total stored across all resource types.
        /// </summary>
        public double TotalStored()
        {
            double total = 0;
            foreach (var kv in _stored)
                total += kv.Value;
            return total;
        }

        /// <summary>
        /// Clears all stored resources (e.g., when zone is lost without evacuation).
        /// </summary>
        public void ClearAll()
        {
            _stored.Clear();
        }

        /// <summary>
        /// Returns all stored resources as a snapshot.
        /// </summary>
        public Dictionary<ResourceType, double> GetAllStored()
        {
            return new Dictionary<ResourceType, double>(_stored);
        }
    }
}
