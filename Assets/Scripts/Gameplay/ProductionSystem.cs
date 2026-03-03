using AutofacUnity;
using UnityEngine;

namespace Siege.Gameplay
{
    public class ProductionSystem : MonoBehaviour
    {
        ResourceManagement _resources;

        void Awake()
        {
            _resources = Resolver.Resolve<ResourceManagement>();
        }

        void Update()
        {
            foreach (var building in Building.All)
            {
                if (!building.IsActive || building.Id == BuildingId.Storage)
                    continue;

                TickProduction(building);
            }
        }

        void TickProduction(Building building)
        {
            if (building.ProductionProgress == 0)
            {
                var recipe = building.GetCurrentRecipe();
                if (recipe == null)
                    return;

                if (!_resources.TryConsumeResources(recipe.Input))
                    return;

                building.InProgressRecipe = recipe;
            }

            building.ProductionProgress += Time.deltaTime;

            if (building.ProductionProgress >= building.InProgressRecipe.Duration)
            {
                _resources.ProduceResources(building.InProgressRecipe.Output);
                building.ProductionProgress = 0;
                building.InProgressRecipe = null;
            }
        }
    }
}
