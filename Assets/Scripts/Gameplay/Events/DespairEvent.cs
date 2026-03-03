using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DespairEvent : GameEvent
    {
        public override string Id => "despair";
        public override string Name => "Wave of Despair";
        public override string Description => "Hopelessness settles over the city like fog. People stop talking. Some stop eating.";
        public override bool IsOneTime => false;
        public override int Priority => 40;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 10 && state.Morale < 45 && Random.value < 0.15f;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Morale -= 10;
            state.Unrest += 8;

            log.Record("Morale", -10, Name);
            log.Record("Unrest", 8, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
