using System.Collections.Generic;
using System.Linq;
using AutofacUnity;
using Siege.Gameplay.UI;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Storage,
    }

    public class Building : MonoBehaviour, IPointerClickHandler
    {
        static readonly List<Building> _all = new();
        public static IReadOnlyList<Building> All => _all;

        public bool IsActive;
        public BuildingId Id;
        public int ActiveRecipeIndex;
        public int AllocatedWorkers { get; set; }

        readonly List<ResourceQuantity> _resources = new();
        public IReadOnlyList<ResourceQuantity> Resources => _resources;

        [System.NonSerialized] public float ProductionProgress;
        [System.NonSerialized] public ProductionRecipe InProgressRecipe;
        BuildingDefinition _definition;

        void OnEnable() => _all.Add(this);
        void OnDisable() => _all.Remove(this);

        public void Add(ResourceType resource, int quantity)
        {
            var index = _resources.FindIndex(r => r.Resource == resource);
            if (index == -1)
            {
                _resources.Add(new ResourceQuantity { Resource = resource, Quantity = quantity });
            }
            else
            {
                var rq = _resources[index];
                rq.Quantity += quantity;
                _resources[index] = rq;
            }
        }

        public void Remove(ResourceType resource, double quantity)
        {
            var index = _resources.FindIndex(r => r.Resource == resource);
            if (index == -1) return;
            var rq = _resources[index];
            rq.Quantity -= quantity;
            _resources[index] = rq;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UISystem.GetExisting<BuildingView>().Show(this);
        }

        public ProductionRecipe GetCurrentRecipe()
        {
            var definition = GetDefinition();
            if (definition?.Recipes == null || definition.Recipes.Length == 0)
                return null;

            var recipe = definition.Recipes[ActiveRecipeIndex];

            if (!recipe.IsAvailable())
            {
                ActiveRecipeIndex = 0;
                recipe = definition.Recipes[ActiveRecipeIndex];
            }

            return recipe;
        }

        public BuildingDefinition GetDefinition()
        {
            return _definition ??= Resolver.Resolve<GameBalance>().Buildings.FirstOrDefault(x => x.Id == Id);
        }
    }
}