using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BlackMarketTraderEvent : GameEvent
    {

        const double MaterialsCostFull = 15.0;
        const double MaterialsCostHaggle = 8.0;
        const double FoodReward = 30.0;
        const double HaggleUnrestPenalty = 5.0;


        public bool HasTriggered { get; set; }
        public string Id => "black_market_trader";
        public string Name => "Black Market Trader";
        public string Description => "A shady figure offers a deal: materials for food.";
        public int Priority => 45;
        public bool IsOneTime => false;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept", $"Give {MaterialsCostFull} Materials, receive {FoodReward} Food."),
            new EventResponse("Haggle", $"Give {MaterialsCostHaggle} Materials, receive {FoodReward} Food. Unrest +{HaggleUnrestPenalty}."),
            new EventResponse("Refuse", "Send the trader away.")
        };

        public string GetNarrativeText(GameState state) =>
            "A cloaked figure slips through the gate at dusk, offering a trade: " +
            "materials for food. The price is steep, but hunger makes beggars of us all.";
    }
}
