using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Manages worker assignment across buildings. Workers are distributed to active buildings
    /// based on priority. The player can manually adjust assignments.
    /// </summary>
    public class WorkerAllocation
    {
        readonly GameState _state;

        public WorkerAllocation(GameState state)
        {
            _state = state;
        }

        /// <summary>
        /// Returns the total workers currently assigned to active buildings.
        /// </summary>
        public int TotalAssigned
        {
            get
            {
                int total = 0;
                foreach (var b in Building.All)
                    if (b.IsActive && !b.NeedsRepair)
                        total += b.AssignedWorkers;
                return total;
            }
        }

        /// <summary>
        /// Returns available (unassigned) healthy workers.
        /// </summary>
        public int AvailableWorkers => Math.Max(0, _state.HealthyWorkers - TotalAssigned);

        /// <summary>
        /// Assign workers to a specific building. Clamps to available workers and max capacity.
        /// </summary>
        public void Assign(Building building, int count)
        {
            if (!building.IsActive || building.NeedsRepair) return;
            if (building.Zone != null && building.Zone.IsLost) return;

            int maxCanAssign = Math.Min(count, building.MaxWorkers);
            int available = AvailableWorkers + building.AssignedWorkers; // reclaim current assignment
            building.AssignedWorkers = Math.Min(maxCanAssign, available);
        }

        /// <summary>
        /// Removes all workers from a building.
        /// </summary>
        public void Unassign(Building building)
        {
            building.AssignedWorkers = 0;
        }

        /// <summary>
        /// Auto-distribute available workers across all active buildings.
        /// Fills buildings in zone order (innermost first for safety) up to their max.
        /// </summary>
        public void AutoAllocate()
        {
            // Clear all assignments
            foreach (var b in Building.All)
                b.AssignedWorkers = 0;

            int available = _state.HealthyWorkers;
            var buildings = GetBuildingsByPriority();

            foreach (var b in buildings)
            {
                if (available <= 0) break;
                if (!b.IsActive || b.NeedsRepair) continue;
                if (b.Zone != null && b.Zone.IsLost) continue;

                int toAssign = Math.Min(b.MaxWorkers, available);
                b.AssignedWorkers = toAssign;
                available -= toAssign;
            }
        }

        /// <summary>
        /// After population changes (deaths, desertion), ensure assignments don't exceed available workers.
        /// Trims from outermost zone buildings first.
        /// </summary>
        public void ValidateAssignments()
        {
            int totalAssigned = TotalAssigned;
            int available = _state.HealthyWorkers;

            if (totalAssigned <= available) return;

            int excess = totalAssigned - available;
            // Remove from outermost zones first (most exposed)
            var buildings = GetBuildingsByPriority();
            for (int i = buildings.Count - 1; i >= 0 && excess > 0; i--)
            {
                var b = buildings[i];
                if (b.AssignedWorkers <= 0) continue;
                int remove = Math.Min(b.AssignedWorkers, excess);
                b.AssignedWorkers -= remove;
                excess -= remove;
            }
        }

        /// <summary>
        /// Returns buildings sorted by priority: innermost (Keep) first, outermost last.
        /// </summary>
        List<Building> GetBuildingsByPriority()
        {
            var list = new List<Building>(Building.All);
            list.Sort((a, b) =>
            {
                int zoneCompare = GetZonePriority(b).CompareTo(GetZonePriority(a));
                if (zoneCompare != 0) return zoneCompare;
                return b.MaxWorkers.CompareTo(a.MaxWorkers); // larger buildings first within zone
            });
            return list;
        }

        // Higher priority = safer zone (Keep=5, OuterFarms=1)
        static int GetZonePriority(Building b) => b.Zone != null ? (int)b.Zone.Id : 0;
    }
}
