using System.Collections.Generic;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Resources
{
    /// <summary>
    /// The single authority for all resource queries and mutations. Aggregates across all
    /// registered ResourceInventory instances. Zone-aware: withdraws from perimeter first,
    /// deposits into innermost zone first.
    /// </summary>
    public class ResourceLedger
    {
        readonly List<(ResourceInventory inventory, Zone zone)> _entries = new();
        readonly List<(ResourceInventory inventory, Zone zone)> _sortBuffer = new();

        // ── Registration ─────────────────────────────────────────────

        public void Register(ResourceInventory inventory, Zone zone)
        {
            if (!_entries.Exists(e => e.inventory == inventory))
                _entries.Add((inventory, zone));
        }

        public void Unregister(ResourceInventory inventory)
        {
            _entries.RemoveAll(e => e.inventory == inventory);
        }

        // ── Queries ──────────────────────────────────────────────────

        /// <summary>Returns total stored amount across all active (non-lost) inventories.</summary>
        public double GetTotal(ResourceType type)
        {
            double total = 0;
            foreach (var (inventory, zone) in _entries)
            {
                if (zone != null && zone.IsLost) continue;
                total += inventory.GetStored(type);
            }
            return total;
        }

        public bool Has(ResourceType type, double amount) =>
            GetTotal(type) >= amount;

        // ── Mutations ────────────────────────────────────────────────

        /// <summary>
        /// Withdraw a resource. Prioritizes the outermost (most exposed) zone first.
        /// Returns the actual amount withdrawn.
        /// </summary>
        public double Withdraw(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;

            BuildSortBuffer();
            _sortBuffer.Sort((a, b) => GetZoneOrder(a.zone).CompareTo(GetZoneOrder(b.zone)));

            double remaining = amount;
            foreach (var (inventory, _) in _sortBuffer)
            {
                if (remaining <= 0) break;
                remaining -= inventory.Withdraw(type, remaining);
            }

            return amount - remaining;
        }

        /// <summary>
        /// Deposit a resource. Fills the innermost (safest) zone first.
        /// Returns the actual amount deposited.
        /// </summary>
        public double Deposit(ResourceType type, double amount)
        {
            if (amount <= 0) return 0;

            BuildSortBuffer();
            _sortBuffer.Sort((a, b) => GetZoneOrder(b.zone).CompareTo(GetZoneOrder(a.zone)));

            double remaining = amount;
            foreach (var (inventory, _) in _sortBuffer)
            {
                if (remaining <= 0) break;
                remaining -= inventory.Deposit(type, remaining);
            }

            return amount - remaining;
        }

        void BuildSortBuffer()
        {
            _sortBuffer.Clear();
            foreach (var entry in _entries)
            {
                if (entry.zone != null && entry.zone.IsLost) continue;
                _sortBuffer.Add(entry);
            }
        }

        static int GetZoneOrder(Zone zone) =>
            zone != null ? (int)zone.Id : int.MaxValue;
    }
}
