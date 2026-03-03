using System;
using System.Collections.Generic;

namespace Siege.Gameplay.Simulation
{
    /// <summary>
    /// Central mutable state for the entire game. One singleton instance, injected everywhere.
    /// All numeric values use double for precision. Systems read/write this directly.
    /// </summary>
    public class GameState
    {
        // ── Starting Value Constants ──────────────────────────────────
        // Tuned so that doing nothing leads to collapse by Day 6–8.

        public const int StartingHealthyWorkers = 40;
        public const int StartingSickWorkers = 5;
        public const int StartingGuards = 8;
        public const int StartingWoundedGuards = 0;
        public const int StartingElderly = 10;

        public const double StartingFood = 120;
        public const double StartingWater = 100;
        public const double StartingFuel = 60;
        public const double StartingMedicine = 30;
        public const double StartingMaterials = 50;

        public const double StartingMorale = 55;
        public const double StartingUnrest = 20;
        public const double StartingSickness = 10;
        public const int StartingSiegeIntensity = 1;

        // ── Day Tracking ─────────────────────────────────────────────

        public int CurrentDay = 1;

        // ── Population ────────────────────────────────────────────────

        public int HealthyWorkers;
        public int SickWorkers;
        public int Guards;
        public int WoundedGuards;
        public int Elderly;

        public int TotalPopulation => HealthyWorkers + SickWorkers + Guards + WoundedGuards + Elderly;
        public int TotalConsumers => TotalPopulation; // everyone consumes food/water

        // ── Resources ─────────────────────────────────────────────────

        public double Food;
        public double Water;
        public double Fuel;
        public double Medicine;
        public double Materials;

        public double GetResource(ResourceType type) => type switch
        {
            ResourceType.Food => Food,
            ResourceType.Water => Water,
            ResourceType.Fuel => Fuel,
            ResourceType.Medicine => Medicine,
            ResourceType.Materials => Materials,
            _ => 0
        };

        public void SetResource(ResourceType type, double value)
        {
            switch (type)
            {
                case ResourceType.Food: Food = value; break;
                case ResourceType.Water: Water = value; break;
                case ResourceType.Fuel: Fuel = value; break;
                case ResourceType.Medicine: Medicine = value; break;
                case ResourceType.Materials: Materials = value; break;
            }
        }

        public void AddResource(ResourceType type, double amount)
        {
            SetResource(type, Math.Max(0, GetResource(type) + amount));
        }

        // ── Status ────────────────────────────────────────────────────

        public double Morale;
        public double Unrest;
        public double Sickness;

        // ── Siege ─────────────────────────────────────────────────────

        public int SiegeIntensity;
        public const int MaxSiegeIntensity = 6;

        // ── Deficit Tracking ──────────────────────────────────────────

        public int ConsecutiveFoodDeficitDays;
        public int ConsecutiveWaterDeficitDays;
        public int ConsecutiveBothDeficitDays; // for loss condition

        // ── Streak Tracking (for positive events) ─────────────────────

        public int ConsecutiveNoDeficitDays;
        public int ConsecutiveLowSicknessDays;
        public int ConsecutiveZoneHeldDays;
        public int ConsecutiveMissionSuccessDays;

        // ── Flags & Tracking ──────────────────────────────────────────

        public bool SignalFireLit;
        public bool FinalAssaultActive;
        public bool PlagueRatsActive;
        public int TaintedWellDays;
        public int ReliefArmyDay;

        public readonly HashSet<string> EnactedLawIds = new();
        public readonly HashSet<string> ActiveToggleOrderIds = new();
        public readonly Dictionary<string, int> OrderCooldowns = new(); // orderId → days remaining
        public readonly HashSet<string> TriggeredEventIds = new();
        public int EventsFiredToday;
        public int OrdersIssuedCount;
        public bool IsGameOver;

        // ── Death Tracking ────────────────────────────────────────────

        public int TotalDeaths;
        public int DeathsToday;

        // ── Zone State ────────────────────────────────────────────────

        public readonly Dictionary<ZoneId, ZoneState> Zones = new();

        // ── Initialization ────────────────────────────────────────────

        public void Initialize()
        {
            CurrentDay = 1;
            HealthyWorkers = StartingHealthyWorkers;
            SickWorkers = StartingSickWorkers;
            Guards = StartingGuards;
            WoundedGuards = StartingWoundedGuards;
            Elderly = StartingElderly;

            Food = StartingFood;
            Water = StartingWater;
            Fuel = StartingFuel;
            Medicine = StartingMedicine;
            Materials = StartingMaterials;

            Morale = StartingMorale;
            Unrest = StartingUnrest;
            Sickness = StartingSickness;
            SiegeIntensity = StartingSiegeIntensity;

            ConsecutiveFoodDeficitDays = 0;
            ConsecutiveWaterDeficitDays = 0;
            ConsecutiveBothDeficitDays = 0;
            ConsecutiveNoDeficitDays = 0;
            ConsecutiveLowSicknessDays = 0;
            ConsecutiveZoneHeldDays = 0;
            ConsecutiveMissionSuccessDays = 0;

            SignalFireLit = false;
            FinalAssaultActive = false;
            PlagueRatsActive = false;
            TaintedWellDays = 0;
            ReliefArmyDay = 0;

            TotalDeaths = 0;
            DeathsToday = 0;
            EventsFiredToday = 0;

            EnactedLawIds.Clear();
            ActiveToggleOrderIds.Clear();
            OrderCooldowns.Clear();
            TriggeredEventIds.Clear();

            InitializeZones();
        }

        void InitializeZones()
        {
            Zones.Clear();
            foreach (ZoneId id in Enum.GetValues(typeof(ZoneId)))
            {
                Zones[id] = new ZoneState(id);
            }
        }

        /// <summary>
        /// Returns the outermost non-lost zone (the active perimeter).
        /// </summary>
        public ZoneId ActivePerimeter
        {
            get
            {
                foreach (ZoneId id in Enum.GetValues(typeof(ZoneId)))
                {
                    if (!Zones[id].IsLost) return id;
                }
                return ZoneId.Keep;
            }
        }

        public int ZonesLostCount
        {
            get
            {
                int count = 0;
                foreach (var z in Zones.Values)
                    if (z.IsLost) count++;
                return count;
            }
        }

        public void ClampValues()
        {
            Morale = Math.Clamp(Morale, 0, 100);
            Unrest = Math.Clamp(Unrest, 0, 100);
            Sickness = Math.Clamp(Sickness, 0, 100);
            SiegeIntensity = Math.Clamp(SiegeIntensity, 1, MaxSiegeIntensity);

            Food = Math.Max(0, Food);
            Water = Math.Max(0, Water);
            Fuel = Math.Max(0, Fuel);
            Medicine = Math.Max(0, Medicine);
            Materials = Math.Max(0, Materials);

            HealthyWorkers = Math.Max(0, HealthyWorkers);
            SickWorkers = Math.Max(0, SickWorkers);
            Guards = Math.Max(0, Guards);
            WoundedGuards = Math.Max(0, WoundedGuards);
            Elderly = Math.Max(0, Elderly);
        }

        public double GetZoneIntegrity(ZoneId id) => Zones[id].Integrity;

        public void SetZoneIntegrity(ZoneId id, double value) => Zones[id].Integrity = value;

        public bool IsZoneLost(ZoneId id) => Zones[id].IsLost;
    }

    /// <summary>
    /// Per-zone mutable state.
    /// </summary>
    public class ZoneState
    {
        public readonly ZoneId Id;
        public double Integrity;
        public int Capacity;
        public int Population;
        public bool IsLost;

        // Defense
        public double BarricadeBuffer;
        public bool HasOilCauldron;
        public bool HasArcherPost;
        public int ArcherPostGuards; // 0-2

        public ZoneState(ZoneId id)
        {
            Id = id;
            Integrity = ZoneDefaults.StartingIntegrity(id);
            Capacity = ZoneDefaults.StartingCapacity(id);
            Population = 0;
            IsLost = false;
        }
    }

    /// <summary>
    /// Default values for zones. Constants per zone.
    /// </summary>
    public static class ZoneDefaults
    {
        public static double StartingIntegrity(ZoneId id) => id switch
        {
            ZoneId.OuterFarms => 80,
            ZoneId.OuterResidential => 90,
            ZoneId.ArtisanQuarter => 95,
            ZoneId.InnerDistrict => 100,
            ZoneId.Keep => 100,
            _ => 100
        };

        public static int StartingCapacity(ZoneId id) => id switch
        {
            ZoneId.OuterFarms => 20,
            ZoneId.OuterResidential => 25,
            ZoneId.ArtisanQuarter => 20,
            ZoneId.InnerDistrict => 15,
            ZoneId.Keep => 10,
            _ => 10
        };

        public static double PerimeterFactor(ZoneId id) => id switch
        {
            ZoneId.OuterFarms => 1.0,
            ZoneId.OuterResidential => 0.9,
            ZoneId.ArtisanQuarter => 0.8,
            ZoneId.InnerDistrict => 0.7,
            ZoneId.Keep => 0.6,
            _ => 1.0
        };
    }
}
