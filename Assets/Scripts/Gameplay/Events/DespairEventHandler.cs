using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DespairEventHandler : EventHandler<DespairEvent>
    {
        public DespairEventHandler(DespairEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 10 && state.Morale < 45 && Random.value < 0.15f;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Morale -= 10;
            state.Unrest += 8;
            log.Record("Morale", -10, Event.Name);
            log.Record("Unrest", 8, Event.Name);
        }
    }
}
