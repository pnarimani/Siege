using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class PlagueRatsEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "plague_rats";
        public string Name => "Plague Rats";
        public string Description => "Rats swarm the lower districts, spreading disease. The people demand action.";
        public int Priority => 70;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Hunt the rats", "Sickness +10, Deaths +2, Unrest +5"),
            new EventResponse("Burn the infested quarter", "Sickness +5, Materials -10"),
            new EventResponse("Do nothing", "Sickness +15, Deaths +3, Unrest +10")
        };

    }
}
