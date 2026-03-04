using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SignalFireEventHandler : EventHandler<SignalFireEvent>
    {
        public SignalFireEventHandler(SignalFireEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 25
            && state.Zones[ZoneId.Keep].Integrity >= 30
            && !state.SignalFireLit
            && state.Fuel >= 5
            && state.Materials >= 15;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Fuel = Math.Max(0, state.Fuel - 5);
                    state.Materials = Math.Max(0, state.Materials - 15);
                    state.Unrest += 5;
                    state.SignalFireLit = true;
                    log.Record("Fuel", -5, Event.Name);
                    log.Record("Materials", -15, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
            }
        }
    }
}
