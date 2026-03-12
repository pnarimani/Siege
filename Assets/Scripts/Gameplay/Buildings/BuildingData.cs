namespace Siege.Gameplay.Buildings
{
    public class BuildingData
    {
        public string Id { get; set; }
        public BuildingCategory Category { get; set; }
    }
    
    public enum BuildingCategory
    {
        Crafting,
        Resource,
        Military,
    }
}