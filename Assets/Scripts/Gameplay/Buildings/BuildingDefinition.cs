using System;

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
            Array.Empty<ResourceQuantity>();

        public ResourceQuantity[] Outputs { get; init; } =
            Array.Empty<ResourceQuantity>();

        public bool IsStorage { get; init; }
        public bool RequiresRepair { get; init; }
        public ProductionRecipe[] Recipes { get; init; } = Array.Empty<ProductionRecipe>();
        public ResourceQuantity[] SalvageMaterials { get; init; } = Array.Empty<ResourceQuantity>();
    }
}