using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DespairEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "despair";
        public string Name => "Wave of Despair";
        public string Description => "Hopelessness settles over the city like fog. People stop talking. Some stop eating.";
        public bool IsOneTime => false;
        public int Priority => 40;

        public string GetNarrativeText(GameState state) => Description;
    }
}
