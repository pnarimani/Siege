using System;
using System.Collections.Generic;

namespace Siege.Gameplay.Resources
{
    public class ResourceInventory
    {
        readonly Dictionary<ResourceType, double> _stored = new();

        public double MaxPerResource { get; set; } = 200;

        public double GetStored(ResourceType type) =>
            _stored.GetValueOrDefault(type, 0);

        /// <summary>
        ///     Deposit resources into this inventory. Returns the amount actually deposited.
        /// </summary>
        public double Deposit(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;
            var current = GetStored(type);
            var space = MaxPerResource - current;
            var deposited = Math.Min(amount, space);
            if (deposited > 0)
                _stored[type] = current + deposited;
            return deposited;
        }

        /// <summary>
        ///     Withdraw resources from this inventory. Returns the amount actually withdrawn.
        /// </summary>
        public double Withdraw(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;
            var current = GetStored(type);
            var withdrawn = Math.Min(amount, current);
            if (withdrawn > 0)
                _stored[type] = current - withdrawn;
            return withdrawn;
        }

        public double AvailableCapacity(ResourceType type) =>
            MaxPerResource - GetStored(type);

        public double TotalStored()
        {
            double total = 0;
            foreach (var kv in _stored)
                total += kv.Value;
            return total;
        }

        public IReadOnlyDictionary<ResourceType, double> GetSnapshot() =>
            _stored;

        public void ClearAll() => _stored.Clear();
    }
}