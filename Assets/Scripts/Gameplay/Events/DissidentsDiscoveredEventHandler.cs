using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DissidentsDiscoveredEventHandler : EventHandler<DissidentsDiscoveredEvent>
    {
        readonly PoliticalState _political;

        public DissidentsDiscoveredEventHandler(DissidentsDiscoveredEvent gameEvent, PoliticalState political) : base(gameEvent)
        {
            _political = political;
        }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 10
            && _political.Tyranny.Value >= 4
            && Random.value < 0.20f;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
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
                    log.Record("Unrest", -15, Event.Name);
                    log.Record("TotalDeaths", 3, Event.Name);
                    log.Record("DeathsToday", 3, Event.Name);
                    log.Record("Morale", -5, Event.Name);
                    break;

                case 1:
                    state.Unrest -= 10;
                    state.Morale += 5;
                    log.Record("Unrest", -10, Event.Name);
                    log.Record("Morale", 5, Event.Name);
                    break;

                case 2:
                    state.Morale += 5;
                    state.Unrest += 8;
                    _political.Faith.Add(1);
                    log.Record("Morale", 5, Event.Name);
                    log.Record("Unrest", 8, Event.Name);
                    break;
            }
        }
    }
}
