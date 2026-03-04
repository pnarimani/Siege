using System;
using System.Collections.Generic;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    ///     Static data defining a building type's base production, costs, and specialization options.
    ///     All values are per-worker-per-day rates unless noted.
    /// </summary>
    public class BuildingDefinition
    {
        public BuildingType Type { get; init; }
        public string Name { get; init; }
        public ZoneId Zone { get; init; }
        public int MaxWorkers { get; init; }

        public ResourceQuantity[] Inputs { get; init; } =
            Array.Empty<ResourceQuantity>(); // consumed per worker per day (legacy/specialization)

        public ResourceQuantity[] Outputs { get; init; } =
            Array.Empty<ResourceQuantity>(); // produced per worker per day (legacy/specialization)

        public bool IsStorage { get; init; }
        public bool RequiresRepair { get; init; }
        public ProductionRecipe[] Recipes { get; init; } = Array.Empty<ProductionRecipe>();
        public ResourceQuantity[] SalvageMaterials { get; init; } = Array.Empty<ResourceQuantity>();

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
            Register(new BuildingDefinition
            {
                Type = BuildingType.Farm, Name = "Farm", Zone = ZoneId.OuterFarms, MaxWorkers = 10,
                Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Food, 3) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Harvest Crops",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 0.5) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Food, 5) },
                        DurationSeconds = 8f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 8) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.HerbGarden, Name = "Herb Garden", Zone = ZoneId.OuterFarms, MaxWorkers = 6,
                Outputs = new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Gather Herbs",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Medicine, 3) },
                        DurationSeconds = 10f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 5) },
            });

            // ── Outer Residential ─────────────────────────────────────
            Register(new BuildingDefinition
            {
                Type = BuildingType.Well, Name = "Well", Zone = ZoneId.OuterResidential, MaxWorkers = 10,
                Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Water, 3) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Draw Water",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 0.5) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Water, 5) },
                        DurationSeconds = 6f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 10) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.FuelStore, Name = "Fuel Store", Zone = ZoneId.OuterResidential, MaxWorkers = 8,
                Outputs = new[] { new ResourceQuantity(ResourceType.Fuel, 2) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Gather Fuel",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Fuel, 4) },
                        DurationSeconds = 7f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 6) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.FieldKitchen, Name = "Field Kitchen", Zone = ZoneId.OuterResidential,
                MaxWorkers = 6,
                Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Food, 2) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Cook Rations",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Fuel, 0.5) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Food, 3) },
                        DurationSeconds = 6f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 5) },
            });

            // ── Artisan Quarter ───────────────────────────────────────
            Register(new BuildingDefinition
            {
                Type = BuildingType.Workshop, Name = "Workshop", Zone = ZoneId.ArtisanQuarter, MaxWorkers = 8,
                Outputs = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Craft Supplies",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Materials, 4) },
                        DurationSeconds = 7f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 12) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.Smithy, Name = "Smithy", Zone = ZoneId.ArtisanQuarter, MaxWorkers = 6,
                Inputs = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Integrity, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Repair Walls",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Integrity, 2) },
                        DurationSeconds = 9f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 15) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.Cistern, Name = "Cistern", Zone = ZoneId.ArtisanQuarter, MaxWorkers = 6,
                Outputs = new[] { new ResourceQuantity(ResourceType.Water, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Collect Rainwater",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Water, 3) },
                        DurationSeconds = 8f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 8) },
            });

            // ── Inner District ────────────────────────────────────────
            Register(new BuildingDefinition
            {
                Type = BuildingType.Clinic, Name = "Clinic", Zone = ZoneId.InnerDistrict, MaxWorkers = 8,
                Inputs = new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Care, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Treat Wounded",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Care, 2) },
                        DurationSeconds = 10f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 6) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.Storehouse, Name = "Storehouse", Zone = ZoneId.InnerDistrict, MaxWorkers = 6,
                Outputs = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                IsStorage = true,
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 10) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.RootCellar, Name = "Root Cellar", Zone = ZoneId.InnerDistrict, MaxWorkers = 4,
                Outputs = new[] { new ResourceQuantity(ResourceType.Food, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Preserve Food",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Food, 2) },
                        DurationSeconds = 8f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 5) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.TradingPost, Name = "Trading Post", Zone = ZoneId.InnerDistrict, MaxWorkers = 0,
                RequiresRepair = true,
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 20) },
            });

            // ── Keep ──────────────────────────────────────────────────
            Register(new BuildingDefinition
            {
                Type = BuildingType.RepairYard, Name = "Repair Yard", Zone = ZoneId.Keep, MaxWorkers = 8,
                Inputs = new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                Outputs = new[] { new ResourceQuantity(ResourceType.Integrity, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Reinforce Defenses",
                        Inputs = new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                        Outputs = new[] { new ResourceQuantity(ResourceType.Integrity, 2) },
                        DurationSeconds = 12f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 18) },
            });
            Register(new BuildingDefinition
            {
                Type = BuildingType.RationingPost, Name = "Rationing Post", Zone = ZoneId.Keep, MaxWorkers = 4,
                Outputs = new[] { new ResourceQuantity(ResourceType.Water, 1) },
                Recipes = new[]
                {
                    new ProductionRecipe
                    {
                        Name = "Distribute Rations",
                        Outputs = new[] { new ResourceQuantity(ResourceType.Water, 2) },
                        DurationSeconds = 6f,
                    },
                },
                SalvageMaterials = new[] { new ResourceQuantity(ResourceType.Materials, 5) },
            });
        }

        static void Register(BuildingDefinition def) => _definitions[def.Type] = def;
    }
}