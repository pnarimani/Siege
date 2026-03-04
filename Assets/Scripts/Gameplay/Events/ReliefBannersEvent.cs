using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEvent : IGameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "relief_banners";
        public string Name => "Banners on the Ridge";
        public string Description => "Your kingdom's banners appear on the eastern ridge.";
        public int Priority => 95;

        public string GetNarrativeText(GameState state) =>
            "Banners appear on the eastern ridge \u2014 your kingdom's colors. " +
            "The relief army is here. Hold one more day.";
    }
}
