using System.Collections.Generic;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Injectable registry for all active StorageBuilding instances.
    /// </summary>
    public class StorageBuildingRegistry
    {
        readonly List<StorageBuilding> _all = new();
        public IReadOnlyList<StorageBuilding> All => _all;

        public void Register(StorageBuilding storage) => _all.Add(storage);
        public void Unregister(StorageBuilding storage) => _all.Remove(storage);
    }
}
