using System;

namespace Siege.Gameplay
{
    public class BuildingDefinition
    {
        public BuildingId Id;
        public bool IsBuilt = true;
        public int MaxWorkers;
        public ZoneId Zone;
        public ProductionRecipe[] Recipes;
    }

    public class ProductionRecipe
    {
        public string Id;
        public Func<bool> IsAvailable;
        public float Duration;
        public ResourceQuantity[] Input;
        public ResourceQuantity[] Output;
    }
}