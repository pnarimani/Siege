using System.Linq;
using AutofacUnity;

namespace Siege.Gameplay
{
    public static class WorkerAllocation
    {
        public static void Reallocate()
        {
            var state = Resolver.Resolve<GameState>();
            if (state == null) return;

            foreach (var b in Building.All)
                b.AllocatedWorkers = 0;

            var eligible = Building.All
                .Where(b => b.IsActive && b.Id != BuildingId.Storage)
                .ToArray();

            if (eligible.Length == 0) return;

            int workers = state.HealthyWorkerCount;
            int perBuilding = workers / eligible.Length;
            int remainder = workers % eligible.Length;

            for (int i = 0; i < eligible.Length; i++)
                eligible[i].AllocatedWorkers = perBuilding + (i < remainder ? 1 : 0);
        }
    }
}
