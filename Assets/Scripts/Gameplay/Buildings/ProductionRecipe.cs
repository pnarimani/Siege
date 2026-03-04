namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Defines one production cycle: inputs consumed, outputs produced, and how long it takes.
    /// A building starts with one recipe; additional recipes can be unlocked via laws.
    /// </summary>
    public class ProductionRecipe
    {
        public string Name { get; init; }
        public ResourceQuantity[] Inputs { get; init; } = System.Array.Empty<ResourceQuantity>();
        public ResourceQuantity[] Outputs { get; init; } = System.Array.Empty<ResourceQuantity>();
        public float DurationSeconds { get; init; } = 5f;

        /// <summary>Law ID that must be enacted to unlock this recipe. Null = always available.</summary>
        public string RequiredLawId { get; init; }
    }
}
