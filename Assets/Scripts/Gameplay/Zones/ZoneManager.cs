using System;
using System.Collections.Generic;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Zones
{
    /// <summary>
    /// Manages zone state: active perimeter tracking, zone loss, evacuation, population distribution.
    /// </summary>
    public class ZoneManager
    {
        readonly GameState _state;
        readonly ChangeLog _changeLog;
        readonly ZoneRegistry _zones;

        // ── Configuration ─────────────────────────────────────────────
        const double SalvagePercentage = 0.6;  // 60% of resources transferred on evacuation
        const int EvacIntegrityThreshold = 40;
        const int EvacSiegeThreshold = 4;

        // Loss shock values (natural fall)
        const double NaturalFallUnrest = 20;
        const double NaturalFallMoraleLoss = 15;
        const double NaturalFallSickness = 10;

        // Controlled evacuation shock
        const double EvacUnrest = 10;
        const double EvacMoraleLoss = 8;

        public event Action<ZoneId> ZoneLost;
        public event Action<ZoneId> ZoneEvacuated;

        public ZoneManager(GameState state, ChangeLog changeLog, ZoneRegistry zones)
        {
            _state = state;
            _changeLog = changeLog;
            _zones = zones;
        }

        // ── Zone Queries ──────────────────────────────────────────────

        public ZoneId ActivePerimeter => _state.ActivePerimeter;

        public ZoneState GetZoneState(ZoneId id) => _state.Zones[id];

        public bool IsZoneLost(ZoneId id) => _state.Zones[id].IsLost;

        // ── Evacuation ────────────────────────────────────────────────

        public bool CanEvacuate(ZoneId id)
        {
            if (id == ZoneId.Keep) return false;
            var zone = _state.Zones[id];
            if (zone.IsLost) return false;

            // Must be the active perimeter (can't skip zones)
            if (id != _state.ActivePerimeter) return false;

            // Check eligibility conditions
            bool allOuterLost = true;
            foreach (ZoneId outerId in ZoneIds.All)
            {
                if ((int)outerId < (int)id && !_state.Zones[outerId].IsLost)
                {
                    allOuterLost = false;
                    break;
                }
            }

            if (allOuterLost) return true;
            if (zone.Integrity < EvacIntegrityThreshold) return true;
            if (_state.SiegeIntensity >= EvacSiegeThreshold) return true;

            return false;
        }

        public void Evacuate(ZoneId id)
        {
            if (!CanEvacuate(id)) return;

            var zone = _state.Zones[id];

            // Salvage resources from storage buildings in this zone
            SalvageResources(id, SalvagePercentage);

            // Mark zone as lost
            LoseZone(id, isEvacuation: true);

            // Apply controlled evacuation shock
            _state.Unrest += EvacUnrest;
            _state.Morale -= EvacMoraleLoss;
            _changeLog.Record("Unrest", EvacUnrest, $"Evacuated {id}");
            _changeLog.Record("Morale", -EvacMoraleLoss, $"Evacuated {id}");

            ZoneEvacuated?.Invoke(id);
        }

        // ── Zone Loss ─────────────────────────────────────────────────

        /// <summary>
        /// Called when a zone's integrity reaches 0 (natural fall) or via evacuation.
        /// </summary>
        public void LoseZone(ZoneId id, bool isEvacuation)
        {
            var zone = _state.Zones[id];
            if (zone.IsLost) return;

            zone.IsLost = true;
            zone.Integrity = 0;

            // Find the Zone MonoBehaviour and deactivate its buildings
            foreach (var z in _zones.All)
            {
                if (z.Id == id)
                {
                    z.OnZoneLost();
                    break;
                }
            }

            if (!isEvacuation)
            {
                // Natural fall — harsher shock, no resource salvage
                _state.Unrest += NaturalFallUnrest;
                _state.Morale -= NaturalFallMoraleLoss;
                _state.Sickness += NaturalFallSickness;
                _changeLog.Record("Unrest", NaturalFallUnrest, $"{id} fell");
                _changeLog.Record("Morale", -NaturalFallMoraleLoss, $"{id} fell");
                _changeLog.Record("Sickness", NaturalFallSickness, $"{id} fell");

                // Lose all resources stored in this zone
                foreach (var z in _zones.All)
                {
                    if (z.Id != id) continue;
                    foreach (var storage in z.GetStorageBuildings())
                        storage.ClearAll();
                }
            }

            // Redistribute population to next surviving zones
            RedistributePopulation(id, zone.Population);

            ZoneLost?.Invoke(id);
        }

        // ── Population Distribution ───────────────────────────────────

        /// <summary>
        /// Distribute population from a lost zone into surviving inner zones.
        /// </summary>
        void RedistributePopulation(ZoneId lostZone, int population)
        {
            var zone = _state.Zones[lostZone];
            zone.Population = 0;

            int remaining = population;
            foreach (ZoneId id in ZoneIds.All)
            {
                if ((int)id <= (int)lostZone) continue;
                var target = _state.Zones[id];
                if (target.IsLost) continue;

                target.Population += remaining;
                remaining = 0;
                break;
            }
        }

        /// <summary>
        /// Distribute the total population across all surviving zones respecting capacity.
        /// Called at game start and when population changes.
        /// </summary>
        public void DistributePopulation()
        {
            int totalPop = _state.TotalPopulation;

            foreach (ZoneId id in ZoneIds.All)
            {
                var zone = _state.Zones[id];
                zone.Population = 0;
            }

            int remaining = totalPop;
            foreach (ZoneId id in ZoneIds.All)
            {
                var zone = _state.Zones[id];
                if (zone.IsLost) continue;

                int assign = Math.Min(remaining, zone.Capacity);
                zone.Population = assign;
                remaining -= assign;

                // If still remaining after filling capacity, overflow stays
                if (remaining <= 0) break;
            }

            // Any remaining overflow goes to the last surviving zone
            if (remaining > 0)
            {
                var zones = ZoneIds.All;
                for (int i = zones.Length - 1; i >= 0; i--)
                {
                    var id = zones[i];
                    if (!_state.Zones[id].IsLost)
                    {
                        _state.Zones[id].Population += remaining;
                        break;
                    }
                }
            }
        }

        // ── Resource Salvage ──────────────────────────────────────────

        void SalvageResources(ZoneId fromZone, double percentage)
        {
            // Collect all resources from storage buildings in the zone
            var salvaged = new Dictionary<ResourceType, double>();

            foreach (var z in _zones.All)
            {
                if (z.Id != fromZone) continue;
                foreach (var storage in z.GetStorageBuildings())
                {
                    var stored = storage.GetAllStored();
                    foreach (var kv in stored)
                    {
                        double amount = kv.Value * percentage;
                        if (!salvaged.ContainsKey(kv.Key))
                            salvaged[kv.Key] = 0;
                        salvaged[kv.Key] += amount;
                    }
                    storage.ClearAll();
                }
            }

            // Deposit salvaged resources into storage buildings in surviving zones
            foreach (var kv in salvaged)
            {
                double toDeposit = kv.Value;
                foreach (var z in _zones.All)
                {
                    if (z.IsLost || z.Id == fromZone) continue;
                    foreach (var storage in z.GetStorageBuildings())
                    {
                        double deposited = storage.Deposit(kv.Key, toDeposit);
                        toDeposit -= deposited;
                        if (toDeposit <= 0) break;
                    }
                    if (toDeposit <= 0) break;
                }
                // Resources that don't fit anywhere are lost — no global pool fallback
            }
        }
    }
}
