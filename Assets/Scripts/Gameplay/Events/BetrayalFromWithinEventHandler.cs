using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BetrayalFromWithinEventHandler : IEventHandler
    {
        const int TriggerDay = 37;
        const int DefectorDivisor = 3;
        const int MercyMoraleBoost = 5;
        const int MaxExecuted = 2;
        const int ExecutionUnrestPenalty = 10;
        const int MinGuardsThreshold = 5;
        const int LowGuardsUnrestPenalty = 15;

        readonly BetrayalFromWithinEvent _event;

        public string EventId => _event.Id;

        public BetrayalFromWithinEventHandler(BetrayalFromWithinEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            int defectors = state.Guards / DefectorDivisor;

            switch (responseIndex)
            {
                case 0:
                    state.Guards -= defectors;
                    state.HealthyWorkers += defectors;
                    state.Morale += MercyMoraleBoost;
                    log.Record("Guards", -defectors, _event.Name);
                    log.Record("HealthyWorkers", defectors, _event.Name);
                    log.Record("Morale", MercyMoraleBoost, _event.Name);
                    break;

                case 1:
                    int executed = Math.Min(MaxExecuted, defectors);
                    int converted = Math.Max(0, defectors - MaxExecuted);
                    state.TotalDeaths += executed;
                    state.DeathsToday += executed;
                    state.Guards -= defectors;
                    state.HealthyWorkers += converted;
                    state.Unrest += ExecutionUnrestPenalty;
                    log.Record("TotalDeaths", executed, _event.Name);
                    log.Record("DeathsToday", executed, _event.Name);
                    log.Record("Guards", -defectors, _event.Name);
                    log.Record("HealthyWorkers", converted, _event.Name);
                    log.Record("Unrest", ExecutionUnrestPenalty, _event.Name);
                    break;

                case 2:
                    state.Guards -= defectors;
                    if (state.Guards < MinGuardsThreshold)
                        state.Unrest += LowGuardsUnrestPenalty;
                    log.Record("Guards", -defectors, _event.Name);
                    if (state.Guards < MinGuardsThreshold)
                        log.Record("Unrest", LowGuardsUnrestPenalty, _event.Name);
                    break;
            }
        }
    }
}
