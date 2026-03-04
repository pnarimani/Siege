using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyUltimatumEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "enemy_ultimatum";
        public string Name => "Enemy Ultimatum";
        public string Description => "The enemy commander sends a final demand: surrender or face annihilation.";
        public int Priority => 80;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Defy them", "Morale +10, Unrest +15"),
            new EventResponse("Negotiate", "Morale -5, Unrest +5, Workers -2 (desertions)"),
            new EventResponse("Ignore", "Morale -15, Unrest +20, Workers -5 (desertions)")
        };

    }
}
