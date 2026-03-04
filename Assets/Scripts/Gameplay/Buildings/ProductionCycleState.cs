using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Drives real-time recipe production on a non-storage building.
    /// Replaces the per-day simulation for buildings that have this component.
    /// </summary>
    public class ProductionCycleState : MonoBehaviour
    {
        Building _building;
        ResourceStorage _storage;
        GameState _state;
        LawDispatcher _lawDispatcher;

        int _selectedRecipeIndex;
        float _elapsed;

        public int SelectedRecipeIndex
        {
            get => _selectedRecipeIndex;
            set
            {
                if (_selectedRecipeIndex == value) return;
                _selectedRecipeIndex = value;
                _elapsed = 0f;
            }
        }

        public float Progress
        {
            get
            {
                var recipe = SelectedRecipe;
                if (recipe == null || recipe.DurationSeconds <= 0) return 0f;
                return Mathf.Clamp01(_elapsed / recipe.DurationSeconds);
            }
        }

        public ProductionRecipe SelectedRecipe
        {
            get
            {
                var available = GetAvailableRecipes();
                if (available.Count == 0) return null;
                int idx = Mathf.Clamp(_selectedRecipeIndex, 0, available.Count - 1);
                return available[idx];
            }
        }

        public List<ProductionRecipe> GetAvailableRecipes()
        {
            var result = new List<ProductionRecipe>();
            if (_building == null) return result;

            foreach (var recipe in _building.Definition.Recipes)
            {
                bool available = recipe.RequiredLawId == null
                    || (_lawDispatcher != null && _lawDispatcher.IsEnacted(recipe.RequiredLawId));
                if (available) result.Add(recipe);
            }
            return result;
        }

        void Awake()
        {
            _building = GetComponent<Building>();
        }

        void Start()
        {
            _storage = Resolver.Resolve<ResourceStorage>();
            _state = Resolver.Resolve<GameState>();
            _lawDispatcher = Resolver.Resolve<LawDispatcher>();
        }

        void Update()
        {
            if (!CanProduce()) return;

            var recipe = SelectedRecipe;
            if (recipe == null) return;

            _elapsed += Time.deltaTime;

            if (_elapsed >= recipe.DurationSeconds)
            {
                _elapsed -= recipe.DurationSeconds;
                TryCompleteCycle(recipe);
            }
        }

        bool CanProduce()
        {
            if (_building == null) return false;
            if (!_building.IsActive) return false;
            if (_building.NeedsRepair) return false;
            if (_building.Zone != null && _building.Zone.IsLost) return false;
            if (_building.AssignedWorkers <= 0) return false;
            return true;
        }

        void TryCompleteCycle(ProductionRecipe recipe)
        {
            // Check inputs
            foreach (var input in recipe.Inputs)
            {
                if (input.Resource == ResourceType.Integrity || input.Resource == ResourceType.Care)
                    continue;
                if (_storage.GetTotal(input.Resource) < input.Quantity)
                    return;
            }

            // Consume inputs
            foreach (var input in recipe.Inputs)
            {
                if (input.Resource == ResourceType.Integrity || input.Resource == ResourceType.Care)
                    continue;
                _storage.Withdraw(input.Resource, input.Quantity);
                _state.AddResource(input.Resource, -input.Quantity);
            }

            // Produce outputs
            foreach (var output in recipe.Outputs)
            {
                if (output.Resource == ResourceType.Integrity)
                {
                    if (_building.Zone != null)
                    {
                        var zoneState = _state.Zones[_building.Zone.Id];
                        zoneState.Integrity = System.Math.Min(100, zoneState.Integrity + output.Quantity);
                    }
                    continue;
                }

                if (output.Resource == ResourceType.Care)
                {
                    _state.AddResource(ResourceType.Care, output.Quantity);
                    continue;
                }

                _storage.Deposit(output.Resource, output.Quantity);
                _state.AddResource(output.Resource, output.Quantity);
            }
        }
    }
}
