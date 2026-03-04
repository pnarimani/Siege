using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DissidentsDiscoveredEventHandler : IEventHandler
    {
        readonly DissidentsDiscoveredEvent _event;

        public string EventId => _event.Id;

        readonly PoliticalState _political;

        public DissidentsDiscoveredEventHandler(DissidentsDiscoveredEvent gameEvent, PoliticalState political)
        {
            _event = gameEvent;
            _political = political;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 10
            && _political.Tyranny.Value >= 4
            && Random.value < 0.20f;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Unrest -= 15;
                    state.TotalDeaths += 3;
                    state.DeathsToday += 3;
                    state.Morale -= 5;
                    _political.Tyranny.Add(1);
                    _political.FearLevel.Add(1);
                    log.Record("Unrest", -15, _event.Name);
                    log.Record("TotalDeaths", 3, _event.Name);
                    log.Record("DeathsToday", 3, _event.Name);
                    log.Record("Morale", -5, _event.Name);
                    break;

                case 1:
                    state.Unrest -= 10;
                    state.Morale += 5;
                    log.Record("Unrest", -10, _event.Name);
                    log.Record("Morale", 5, _event.Name);
                    break;

                case 2:
                    state.Morale += 5;
                    state.Unrest += 8;
                    _political.Faith.Add(1);
                    log.Record("Morale", 5, _event.Name);
                    log.Record("Unrest", 8, _event.Name);
                    break;
            }
        }
    }
}
