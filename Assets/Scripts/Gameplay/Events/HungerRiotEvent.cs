using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HungerRiotEvent : GameEvent
    {
        public override string Id => "hunger_riot";
        public override string Name => "Hunger Riot";
        public override string Description => "Angry mobs force the granary doors. What little remains is trampled and scattered in the chaos.";
        public override bool IsOneTime => true;
        public override int Priority => 80;

        public override bool CanTrigger(GameState state)
        {
            return state.ConsecutiveFoodDeficitDays >= 2 && state.Unrest > 50;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Food = Math.Max(0, state.Food - 80);
            state.Unrest += 15;

            int guardsLost = Math.Min(5, state.Guards);
            state.Guards -= guardsLost;

            int deaths = guardsLost;
            state.TotalDeaths += deaths;
            state.DeathsToday += deaths;

            log.Record("Food", -80, Name);
            log.Record("Unrest", 15, Name);
            log.Record("Guards", -guardsLost, Name);
            log.Record("TotalDeaths", deaths, Name);
            log.Record("DeathsToday", deaths, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
