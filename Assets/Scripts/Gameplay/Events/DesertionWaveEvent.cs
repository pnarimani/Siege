using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DesertionWaveEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "desertion_wave";
        public string Name => "Desertion Wave";
        public string Description => "At dawn, the western gate stands open. Footprints in the mud lead away into the fog.";
        public bool IsOneTime => true;
        public int Priority => 70;

        public string GetNarrativeText(GameState state) => Description;
    }
}
