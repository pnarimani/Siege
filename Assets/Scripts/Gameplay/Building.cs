using System;
using System.Collections.Generic;
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
        public bool IsEnabled;
        public BuildingId Id;
        public int ActiveRecipeIndex;

        readonly List<ResourceQuantity> _resources = new();
        public IReadOnlyList<ResourceQuantity> Resources => _resources;

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

        public void OnPointerClick(PointerEventData eventData)
        {
            UISystem.Open<BuildingView>(UILayer.Window).Show(this);
        }
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
}
