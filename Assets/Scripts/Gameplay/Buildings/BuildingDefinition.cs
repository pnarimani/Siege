using System.Collections.Generic;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Static data defining a building type's base production, costs, and specialization options.
    /// All values are per-worker-per-day rates unless noted.
    /// </summary>
    public class BuildingDefinition
    {
        public readonly BuildingType Type;
        public readonly string Name;
        public readonly ZoneId Zone;
        public readonly int MaxWorkers;
        public readonly ResourceQuantity[] Inputs;  // consumed per worker per day
        public readonly ResourceQuantity[] Outputs;  // produced per worker per day
        public readonly bool IsStorage;
        public readonly bool RequiresRepair; // e.g., Trading Post

        public BuildingDefinition(
            BuildingType type, string name, ZoneId zone, int maxWorkers,
            ResourceQuantity[] inputs, ResourceQuantity[] outputs,
            bool isStorage = false, bool requiresRepair = false)
        {
            Type = type;
            Name = name;
            Zone = zone;
            MaxWorkers = maxWorkers;
            Inputs = inputs;
            Outputs = outputs;
            IsStorage = isStorage;
            RequiresRepair = requiresRepair;
        }

        // ── All Building Definitions ──────────────────────────────────

        static Dictionary<BuildingType, BuildingDefinition> _definitions;

        public static BuildingDefinition Get(BuildingType type)
        {
            if (_definitions == null) InitializeDefinitions();
            return _definitions[type];
        }

        public static IReadOnlyDictionary<BuildingType, BuildingDefinition> All
        {
            get
            {
                if (_definitions == null) InitializeDefinitions();
                return _definitions;
            }
        }

        static void InitializeDefinitions()
        {
            _definitions = new Dictionary<BuildingType, BuildingDefinition>();

            // ── Outer Farms ───────────────────────────────────────────
            Register(new BuildingDefinition(
                BuildingType.Farm, "Farm", ZoneId.OuterFarms, 10,
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 3) }
            ));
            Register(new BuildingDefinition(
                BuildingType.HerbGarden, "Herb Garden", ZoneId.OuterFarms, 6,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Medicine, 1) }
            ));

            // ── Outer Residential ─────────────────────────────────────
            Register(new BuildingDefinition(
                BuildingType.Well, "Well", ZoneId.OuterResidential, 10,
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Water, 3) }
            ));
            Register(new BuildingDefinition(
                BuildingType.FuelStore, "Fuel Store", ZoneId.OuterResidential, 8,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Fuel, 2) }
            ));
            Register(new BuildingDefinition(
                BuildingType.FieldKitchen, "Field Kitchen", ZoneId.OuterResidential, 6,
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 2) }
            ));

            // ── Artisan Quarter ───────────────────────────────────────
            Register(new BuildingDefinition(
                BuildingType.Workshop, "Workshop", ZoneId.ArtisanQuarter, 8,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Materials, 2) }
            ));
            Register(new BuildingDefinition(
                BuildingType.Smithy, "Smithy", ZoneId.ArtisanQuarter, 6,
                new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 1) }
            ));
            Register(new BuildingDefinition(
                BuildingType.Cistern, "Cistern", ZoneId.ArtisanQuarter, 6,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Water, 1) }
            ));

            // ── Inner District ────────────────────────────────────────
            Register(new BuildingDefinition(
                BuildingType.Clinic, "Clinic", ZoneId.InnerDistrict, 8,
                new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                new[] { new ResourceQuantity(ResourceType.Care, 1) }
            ));
            Register(new BuildingDefinition(
                BuildingType.Storehouse, "Storehouse", ZoneId.InnerDistrict, 6,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                isStorage: true
            ));
            Register(new BuildingDefinition(
                BuildingType.RootCellar, "Root Cellar", ZoneId.InnerDistrict, 4,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Food, 1) }
            ));
            Register(new BuildingDefinition(
                BuildingType.TradingPost, "Trading Post", ZoneId.InnerDistrict, 0,
                System.Array.Empty<ResourceQuantity>(),
                System.Array.Empty<ResourceQuantity>(),
                requiresRepair: true
            ));

            // ── Keep ──────────────────────────────────────────────────
            Register(new BuildingDefinition(
                BuildingType.RepairYard, "Repair Yard", ZoneId.Keep, 8,
                new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 1) }
            ));
            Register(new BuildingDefinition(
                BuildingType.RationingPost, "Rationing Post", ZoneId.Keep, 4,
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Water, 1) }
            ));
        }

        static void Register(BuildingDefinition def) => _definitions[def.Type] = def;
    }
}
