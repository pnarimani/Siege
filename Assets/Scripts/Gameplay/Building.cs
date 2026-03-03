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
        public bool IsActive;
        public BuildingId Id;
        public int ActiveRecipeIndex;
        public int AllocatedWorkers { get; set; }

        readonly List<ResourceQuantity> _resources = new();
        public IReadOnlyList<ResourceQuantity> Resources => _resources;

        float _productionProgress;
        ProductionRecipe _inProgressRecipe;

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

        void Update()
        {
            if (!IsActive)
                return;

            if (Id == BuildingId.Storage)
                return;

            if (_productionProgress == 0)
            {
                var recipe = GetCurrentRecipe();
                if (recipe == null)
                    return;

                if (!Resolver.Resolve<ResourceManagement>().TryConsumeResources(recipe.Input))
                {
                    return;
                }

                _inProgressRecipe = recipe;
            }
            
            _productionProgress += Time.deltaTime;
            
            if (_productionProgress >= _inProgressRecipe.Duration)
            {
                Resolver.Resolve<ResourceManagement>().ProduceResources(_inProgressRecipe.Output);
                _productionProgress = 0;
                _inProgressRecipe = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UISystem.GetExisting<BuildingView>().Show(this);
        }

        ProductionRecipe GetCurrentRecipe()
        {
            var definition = GetDefinition();
            if (definition.Recipes == null || definition.Recipes.Length == 0)
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
            return Resolver.Resolve<GameBalance>().Buildings.FirstOrDefault(x => x.Id == Id);
        }
    }
}