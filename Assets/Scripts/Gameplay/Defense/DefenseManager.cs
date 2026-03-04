using System;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Defense
{
    /// <summary>
    /// Manages building and maintaining defenses in zones.
    /// </summary>
    public class DefenseManager
    {
        // Barricade
        const double BarricadeMaterialCost = 15;
        const double BarricadeBuffer = 12;

        // Oil Cauldron
        const double CauldronFuelCost = 10;
        const double CauldronMaterialCost = 10;

        // Archer Post
        const double ArcherPostMaterialCost = 20;
        const int ArcherPostGuardsRequired = 2;

        readonly GameState _state;
        readonly ChangeLog _changeLog;
        readonly ResourceStorage _storage;

        public DefenseManager(GameState state, ChangeLog changeLog, ResourceStorage storage)
        {
            _state = state;
            _changeLog = changeLog;
            _storage = storage;
        }

        // ── Barricade ─────────────────────────────────────────────────

        public bool CanBuildBarricade(ZoneId zone)
        {
            if (_state.Zones[zone].IsLost) return false;
            return _state.Materials >= BarricadeMaterialCost;
        }

        public void BuildBarricade(ZoneId zone)
        {
            if (!CanBuildBarricade(zone)) return;

            _storage.Withdraw(ResourceType.Materials, BarricadeMaterialCost);
            _state.Materials -= BarricadeMaterialCost;
            _state.Zones[zone].BarricadeBuffer += BarricadeBuffer;
            _changeLog.Record("Materials", -BarricadeMaterialCost, "Build barricade");
            _changeLog.Record("Barricade", BarricadeBuffer, $"Barricade ({zone})");
        }

        // ── Oil Cauldron ──────────────────────────────────────────────

        public bool CanBuildOilCauldron(ZoneId zone)
        {
            var zoneState = _state.Zones[zone];
            if (zoneState.IsLost) return false;
            if (zoneState.HasOilCauldron) return false;
            return _state.Fuel >= CauldronFuelCost && _state.Materials >= CauldronMaterialCost;
        }

        public void BuildOilCauldron(ZoneId zone)
        {
            if (!CanBuildOilCauldron(zone)) return;

            _storage.Withdraw(ResourceType.Fuel, CauldronFuelCost);
            _storage.Withdraw(ResourceType.Materials, CauldronMaterialCost);
            _state.Fuel -= CauldronFuelCost;
            _state.Materials -= CauldronMaterialCost;
            _state.Zones[zone].HasOilCauldron = true;
            _changeLog.Record("Fuel", -CauldronFuelCost, "Build oil cauldron");
            _changeLog.Record("Materials", -CauldronMaterialCost, "Build oil cauldron");
        }

        // ── Archer Post ───────────────────────────────────────────────

        public bool CanBuildArcherPost(ZoneId zone)
        {
            var zoneState = _state.Zones[zone];
            if (zoneState.IsLost) return false;
            if (zoneState.HasArcherPost) return false;
            return _state.Materials >= ArcherPostMaterialCost;
        }

        public void BuildArcherPost(ZoneId zone)
        {
            if (!CanBuildArcherPost(zone)) return;

            _storage.Withdraw(ResourceType.Materials, ArcherPostMaterialCost);
            _state.Materials -= ArcherPostMaterialCost;
            _state.Zones[zone].HasArcherPost = true;
            _changeLog.Record("Materials", -ArcherPostMaterialCost, "Build archer post");
        }

        public bool CanAssignArcherGuards(ZoneId zone, int count)
        {
            var zoneState = _state.Zones[zone];
            if (!zoneState.HasArcherPost) return false;
            if (count < 0 || count > ArcherPostGuardsRequired) return false;

            int currentAssigned = zoneState.ArcherPostGuards;
            int delta = count - currentAssigned;
            if (delta > 0 && _state.Guards < delta) return false;

            return true;
        }

        public void AssignArcherGuards(ZoneId zone, int count)
        {
            if (!CanAssignArcherGuards(zone, count)) return;

            var zoneState = _state.Zones[zone];
            int delta = count - zoneState.ArcherPostGuards;
            zoneState.ArcherPostGuards = count;

            // Guards assigned to archer posts are still guards, just committed
            _changeLog.Record("ArcherGuards", delta, $"Archer post ({zone})");
        }
    }
}
