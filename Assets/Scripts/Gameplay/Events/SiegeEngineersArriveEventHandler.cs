using System;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SiegeEngineersArriveEventHandler : IEventHandler
    {
        readonly SiegeEngineersArriveEvent _event;

        public string EventId => _event.Id;

        readonly PoliticalState _political;

        public SiegeEngineersArriveEventHandler(SiegeEngineersArriveEvent gameEvent, PoliticalState political)
        {
            _event = gameEvent;
            _political = political;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 15
            && _political.Fortification.Value >= 5
            && UnityEngine.Random.value < 0.25f;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers += 3;
                    state.Materials += 20;
                    state.Food = Math.Max(0, state.Food - 10);
                    _political.Fortification.Add(1);
                    log.Record("HealthyWorkers", 3, _event.Name);
                    log.Record("Materials", 20, _event.Name);
                    log.Record("Food", -10, _event.Name);
                    break;

                case 1:
                    state.Morale += 5;
                    log.Record("Morale", 5, _event.Name);
                    break;
            }
        }
    }
}
