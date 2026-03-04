using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BetrayalFromWithinEventHandler : EventHandler<BetrayalFromWithinEvent>
    {
        public BetrayalFromWithinEventHandler(BetrayalFromWithinEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 37;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            int defectors = state.Guards / 3;

            switch (responseIndex)
            {
                case 0:
                    state.Guards -= defectors;
                    state.HealthyWorkers += defectors;
                    state.Morale += 5;
                    log.Record("Guards", -defectors, Event.Name);
                    log.Record("HealthyWorkers", defectors, Event.Name);
                    log.Record("Morale", 5, Event.Name);
                    break;

                case 1:
                    int executed = Math.Min(2, defectors);
                    int converted = Math.Max(0, defectors - 2);
                    state.TotalDeaths += executed;
                    state.DeathsToday += executed;
                    state.Guards -= defectors;
                    state.HealthyWorkers += converted;
                    state.Unrest += 10;
                    log.Record("TotalDeaths", executed, Event.Name);
                    log.Record("DeathsToday", executed, Event.Name);
                    log.Record("Guards", -defectors, Event.Name);
                    log.Record("HealthyWorkers", converted, Event.Name);
                    log.Record("Unrest", 10, Event.Name);
                    break;

                case 2:
                    state.Guards -= defectors;
                    if (state.Guards < 5)
                        state.Unrest += 15;
                    log.Record("Guards", -defectors, Event.Name);
                    if (state.Guards < 5)
                        log.Record("Unrest", 15, Event.Name);
                    break;
            }
        }
    }
}
