using System;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class ChildrensPleaEventHandler : EventHandler<ChildrensPleaEvent>
    {
        readonly PoliticalState _political;

        public ChildrensPleaEventHandler(ChildrensPleaEvent gameEvent, PoliticalState political) : base(gameEvent)
        {
            _political = political;
        }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 12
            && _political.Faith.Value >= 3
            && UnityEngine.Random.value < 0.15f;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Materials = Math.Max(0, state.Materials - 10);
                    state.Morale += 10;
                    state.Sickness += 3;
                    _political.Faith.Add(1);
                    log.Record("Materials", -10, Event.Name);
                    log.Record("Morale", 10, Event.Name);
                    log.Record("Sickness", 3, Event.Name);
                    break;

                case 1:
                    state.Morale -= 5;
                    state.Unrest += 5;
                    _political.Tyranny.Add(1);
                    log.Record("Morale", -5, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
            }
        }
    }
}
