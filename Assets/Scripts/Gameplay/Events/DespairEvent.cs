using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DespairEvent : IGameEvent
    {
        public string Id => "despair";
        public string Name => "Wave of Despair";
        public string Description => "Hopelessness settles over the city like fog. People stop talking. Some stop eating.";

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 10 && state.Morale < 45 && Random.value < 0.15f;

        public void Execute(GameState state, ChangeLog log)
        {
            state.Morale -= 10;
            state.Unrest += 8;
            log.Record("Morale", -10, Name);
            log.Record("Unrest", 8, Name);
        }

        public string GetNarrativeText(GameState state) => Description;

        public IGameEvent Clone() => new DespairEvent();
    }
}
