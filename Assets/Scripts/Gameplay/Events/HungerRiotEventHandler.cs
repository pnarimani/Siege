using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HungerRiotEventHandler : EventHandler<HungerRiotEvent>
    {
        public HungerRiotEventHandler(HungerRiotEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveFoodDeficitDays >= 2 && state.Unrest > 50;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Food = Math.Max(0, state.Food - 80);
            state.Unrest += 15;
            int guardsLost = Math.Min(5, state.Guards);
            state.Guards -= guardsLost;
            int deaths = guardsLost;
            state.TotalDeaths += deaths;
            state.DeathsToday += deaths;
            log.Record("Food", -80, Event.Name);
            log.Record("Unrest", 15, Event.Name);
            log.Record("Guards", -guardsLost, Event.Name);
            log.Record("TotalDeaths", deaths, Event.Name);
            log.Record("DeathsToday", deaths, Event.Name);
        }
    }
}
