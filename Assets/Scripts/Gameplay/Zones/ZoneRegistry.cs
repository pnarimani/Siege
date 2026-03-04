using System.Collections.Generic;

namespace Siege.Gameplay.Zones
{
    /// <summary>
    /// Injectable registry for all active Zone instances.
    /// </summary>
    public class ZoneRegistry
    {
        readonly List<Zone> _all = new();
        public IReadOnlyList<Zone> All => _all;

        public void Register(Zone zone) => _all.Add(zone);
        public void Unregister(Zone zone) => _all.Remove(zone);
    }
}
