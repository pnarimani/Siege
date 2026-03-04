using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FortuneFavorsBoldEvent : IGameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "fortune_favors_bold";
        public string Name => "Fortune Favors the Bold";
        public string Description => "Recent victories inspire volunteers.";
        public int Priority => 20;
        public bool IsOneTime => false;

        public string GetNarrativeText(GameState state) =>
            "Inspired by recent victories, 2 volunteers join the workforce.";
    }
}
