using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SpySellingIntelEvent : GameEvent
    {

        const double MaterialsCost = 10.0;
        const double FoodCost = 5.0;


        public bool HasTriggered { get; set; }
        public string Id => "spy_selling_intel";
        public string Name => "Spy Selling Intel";
        public string Description => "A spy offers intelligence on enemy movements.";
        public int Priority => 50;
        public bool IsOneTime => false;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Buy Intel",
                $"Materials -{MaterialsCost}, Food -{FoodCost}. Siege intensity decreases.",
                "The spy's information is detailed and credible. Siege pressure eases."),
            new EventResponse("Turn Away", "Send the spy away empty-handed.")
        };

        public string GetNarrativeText(GameState state) =>
            "A figure in a threadbare cloak approaches your guard captain, " +
            "claiming to have intelligence on the enemy's plans — for a price.";
    }
}
