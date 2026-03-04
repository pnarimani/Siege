using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HungerRiotEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "hunger_riot";
        public string Name => "Hunger Riot";
        public string Description => "Angry mobs force the granary doors. What little remains is trampled and scattered in the chaos.";
        public bool IsOneTime => true;
        public int Priority => 80;

        public string GetNarrativeText(GameState state) => Description;
    }
}
