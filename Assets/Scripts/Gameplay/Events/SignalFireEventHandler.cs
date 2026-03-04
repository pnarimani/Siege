using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SignalFireEventHandler : IEventHandler
    {
        const int StartDay = 25;
        const double MinKeepIntegrity = 30;
        const int FuelCost = 5;
        const int MaterialCost = 15;
        const int LightingUnrest = 5;

        readonly SignalFireEvent _event;

        public string EventId => _event.Id;

        public SignalFireEventHandler(SignalFireEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= StartDay
            && state.Zones[ZoneId.Keep].Integrity >= MinKeepIntegrity
            && !state.SignalFireLit
            && state.Fuel >= FuelCost
            && state.Materials >= MaterialCost;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Fuel = Math.Max(0, state.Fuel - FuelCost);
                    state.Materials = Math.Max(0, state.Materials - MaterialCost);
                    state.Unrest += LightingUnrest;
                    state.SignalFireLit = true;
                    log.Record("Fuel", -FuelCost, _event.Name);
                    log.Record("Materials", -MaterialCost, _event.Name);
                    log.Record("Unrest", LightingUnrest, _event.Name);
                    break;
            }
        }
    }
}
