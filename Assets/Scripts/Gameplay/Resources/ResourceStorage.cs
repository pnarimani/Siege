using System;
using System.Collections.Generic;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Resources
{
    /// <summary>
    /// Manages physical resource storage across StorageBuildings.
    /// Withdrawal prioritizes the active perimeter zone, then inward.
    /// Deposit fills from innermost zone outward (safer first).
    /// </summary>
    public class ResourceStorage
    {
        readonly Simulation.GameState _state;
        readonly StorageBuildingRegistry _storages;
        readonly List<StorageBuilding> _withdrawBuffer = new();
        readonly List<StorageBuilding> _depositBuffer = new();

        public ResourceStorage(Simulation.GameState state, StorageBuildingRegistry storages)
        {
            _state = state;
            _storages = storages;
        }

        /// <summary>
        /// Returns total stored amount of a resource across all active storage buildings.
        /// </summary>
        public double GetTotal(ResourceType type)
        {
            double total = 0;
            foreach (var storage in _storages.All)
            {
                if (storage.Building != null && storage.Building.Zone != null && storage.Building.Zone.IsLost)
                    continue;
                total += storage.GetStored(type);
            }
            return total;
        }

        /// <summary>
        /// Withdraw a resource amount. Prioritizes perimeter zone first, then moves inward.
        /// Returns the actual amount withdrawn.
        /// </summary>
        public double Withdraw(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;

            double remaining = amount;
            var ordered = GetStorageByWithdrawPriority();

            foreach (var storage in ordered)
            {
                if (remaining <= 0) break;
                double withdrawn = storage.Withdraw(type, remaining);
                remaining -= withdrawn;
            }

            return amount - remaining;
        }

        /// <summary>
        /// Deposit a resource amount. Fills innermost (safest) storage first.
        /// Returns the actual amount deposited.
        /// </summary>
        public double Deposit(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;

            double remaining = amount;
            var ordered = GetStorageByDepositPriority();

            foreach (var storage in ordered)
            {
                if (remaining <= 0) break;
                double deposited = storage.Deposit(type, remaining);
                remaining -= deposited;
            }

            return amount - remaining;
        }

        /// <summary>
        /// For withdrawal: perimeter (outermost active) first, then inward.
        /// This means exposed resources get used first before safer inner reserves.
        /// </summary>
        List<StorageBuilding> GetStorageByWithdrawPriority()
        {
            _withdrawBuffer.Clear();
            foreach (var s in _storages.All)
            {
                if (s.Building == null || s.Building.Zone == null) continue;
                if (s.Building.Zone.IsLost) continue;
                _withdrawBuffer.Add(s);
            }

            // Sort by zone: outermost first (lower ZoneId = more exposed)
            _withdrawBuffer.Sort((a, b) => ((int)a.Building.Zone.Id).CompareTo((int)b.Building.Zone.Id));
            return _withdrawBuffer;
        }

        /// <summary>
        /// For deposit: innermost (safest) first.
        /// </summary>
        List<StorageBuilding> GetStorageByDepositPriority()
        {
            _depositBuffer.Clear();
            foreach (var s in _storages.All)
            {
                if (s.Building == null || s.Building.Zone == null) continue;
                if (s.Building.Zone.IsLost) continue;
                _depositBuffer.Add(s);
            }

            // Sort by zone: innermost first (higher ZoneId = safer)
            _depositBuffer.Sort((a, b) => ((int)b.Building.Zone.Id).CompareTo((int)a.Building.Zone.Id));
            return _depositBuffer;
        }
    }
}
