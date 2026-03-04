using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefHornsEvent : IGameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "relief_horns";
        public string Name => "War Horns Beyond the Hills";
        public string Description => "War horns echo from beyond the hills.";
        public int Priority => 95;

        public string GetNarrativeText(GameState state) =>
            "The unmistakable sound of war horns echoes from beyond the hills. " +
            "Someone is coming. Friend or foe, you cannot yet tell.";
    }
}
