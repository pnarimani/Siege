using System;
using UnityEngine;

namespace Siege.Gameplay
{
    public enum BuildingId
    {
        Farm,
        HerbGarden,
        Well,
        FuelStore,
        FieldKitchen,
        Workshop,
        Smithy,
        Cistern,
        Clinic,
        Storehouse,
        RootCellar,
        RepairYard,
        RationingPost,
        TradingPost,
    }

    public class Building : MonoBehaviour
    {
        public bool IsEnabled;
        public BuildingId Id;
        public int ActiveRecipeIndex;
    }

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
        public ResourceQuantity[] Input;
        public ResourceQuantity[] Output;
    }

    public enum ResourceKind
    {
        Food,
        Water,
        Fuel,
        Medicine,
        Materials,
        Integrity,
        Care,
    }
}
