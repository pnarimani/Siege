using System.Linq;
using UnityEngine;

namespace Siege.Gameplay
{
    public static class WorkerAllocation
    {
        public static void Reallocate()
        {
            var buildings = Object.FindObjectsByType<Building>(FindObjectsSortMode.None);
            var state = GameState.Current;
            if (state == null) return;

            var eligible = buildings
                .Where(b => b.IsActive && b.Id != BuildingId.Storage)
                .ToArray();

            foreach (var b in buildings)
                b.AllocatedWorkers = 0;

            if (eligible.Length == 0) return;

            int workers = state.HealthyWorkerCount;
            int perBuilding = workers / eligible.Length;
            int remainder = workers % eligible.Length;

            for (int i = 0; i < eligible.Length; i++)
                eligible[i].AllocatedWorkers = perBuilding + (i < remainder ? 1 : 0);
        }
    }
}
