using System;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CrisisOfFaithEventHandler : IEventHandler
    {
        readonly CrisisOfFaithEvent _event;

        public string EventId => _event.Id;

        readonly PoliticalState _political;

        public CrisisOfFaithEventHandler(CrisisOfFaithEvent gameEvent, PoliticalState political)
        {
            _event = gameEvent;
            _political = political;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 15
            && _political.Faith.Value >= 6
            && state.Morale < 30;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += 20;
                    state.Food = Math.Max(0, state.Food - 10);
                    state.Sickness += 5;
                    _political.Faith.Add(1);
                    log.Record("Morale", 20, _event.Name);
                    log.Record("Food", -10, _event.Name);
                    log.Record("Sickness", 5, _event.Name);
                    break;

                case 1:
                    state.Morale -= 5;
                    state.Unrest += 10;
                    _political.Faith.Add(-3);
                    log.Record("Morale", -5, _event.Name);
                    log.Record("Unrest", 10, _event.Name);
                    break;
            }
        }
    }
}
