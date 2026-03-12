using System.Collections.Generic;
using Siege.Gameplay.Buildings;

namespace Siege.Gameplay
{
    public class GameData
    {
        public List<BuildingData> Buildings { get; } = new()
        {
            new BuildingData { Id = "CraftingStation", Category = BuildingCategory.Crafting },
            new BuildingData { Id = "Campfire", Category = BuildingCategory.Crafting },
        };
    }
}