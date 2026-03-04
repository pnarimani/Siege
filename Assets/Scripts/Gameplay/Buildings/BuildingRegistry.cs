using System.Collections.Generic;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Injectable registry for all active Building instances.
    /// Buildings register/unregister themselves in OnEnable/OnDisable.
    /// </summary>
    public class BuildingRegistry
    {
        readonly List<Building> _all = new();
        public IReadOnlyList<Building> All => _all;

        public void Register(Building building) => _all.Add(building);
        public void Unregister(Building building) => _all.Remove(building);
    }
}
