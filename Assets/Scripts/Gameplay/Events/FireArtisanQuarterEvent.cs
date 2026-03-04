using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FireArtisanQuarterEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "fire_artisan_quarter";
        public string Name => "Fire in the Artisan Quarter";
        public string Description => "Flames engulf the workshops. The smell of burning timber and lacquer fills the sky.";
        public bool IsOneTime => true;
        public int Priority => 60;

        public string GetNarrativeText(GameState state) => Description;
    }
}
