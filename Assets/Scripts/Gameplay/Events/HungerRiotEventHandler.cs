using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HungerRiotEventHandler : IEventHandler
    {
        readonly HungerRiotEvent _event;

        public string EventId => _event.Id;

        public HungerRiotEventHandler(HungerRiotEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveFoodDeficitDays >= 2 && state.Unrest > 50;

        public void Execute(GameState state, ChangeLog log)
        {
            state.Food = Math.Max(0, state.Food - 80);
            state.Unrest += 15;
            int guardsLost = Math.Min(5, state.Guards);
            state.Guards -= guardsLost;
            int deaths = guardsLost;
            state.TotalDeaths += deaths;
            state.DeathsToday += deaths;
            log.Record("Food", -80, _event.Name);
            log.Record("Unrest", 15, _event.Name);
            log.Record("Guards", -guardsLost, _event.Name);
            log.Record("TotalDeaths", deaths, _event.Name);
            log.Record("DeathsToday", deaths, _event.Name);
        }
    }
}
