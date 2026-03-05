using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BetrayalFromWithinEvent : IGameEvent
    {
        const int TriggerDay = 37;
        const int DefectorDivisor = 3;
        const int MercyMoraleBoost = 5;
        const int MaxExecuted = 2;
        const int ExecutionUnrestPenalty = 10;
        const int MinGuardsThreshold = 5;
        const int LowGuardsUnrestPenalty = 15;

        bool _hasTriggered;

        public string Id => "betrayal_from_within";
        public string Name => "Betrayal from Within";
        public string Description => "A faction of guards has been meeting in secret. They plan to open the gates at dawn. You learn of it just in time.";

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Offer amnesty",
                    "Defectors rejoin as workers. +5 Morale."),
                new EventResponse(
                    "Make an example",
                    "Execute the ringleaders. The rest become workers. +10 Unrest."),
                new EventResponse(
                    "Let them go",
                    "Lose the defectors entirely. Risk unrest if garrison thins.")
            };
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            int defectors = state.Guards / DefectorDivisor;

            switch (responseIndex)
            {
                case 0:
                    state.Guards -= defectors;
                    state.HealthyWorkers += defectors;
                    state.Morale += MercyMoraleBoost;
                    log.Record("Guards", -defectors, Name);
                    log.Record("HealthyWorkers", defectors, Name);
                    log.Record("Morale", MercyMoraleBoost, Name);
                    break;

                case 1:
                    int executed = System.Math.Min(MaxExecuted, defectors);
                    int converted = System.Math.Max(0, defectors - MaxExecuted);
                    state.TotalDeaths += executed;
                    state.DeathsToday += executed;
                    state.Guards -= defectors;
                    state.HealthyWorkers += converted;
                    state.Unrest += ExecutionUnrestPenalty;
                    log.Record("TotalDeaths", executed, Name);
                    log.Record("DeathsToday", executed, Name);
                    log.Record("Guards", -defectors, Name);
                    log.Record("HealthyWorkers", converted, Name);
                    log.Record("Unrest", ExecutionUnrestPenalty, Name);
                    break;

                case 2:
                    state.Guards -= defectors;
                    if (state.Guards < MinGuardsThreshold)
                        state.Unrest += LowGuardsUnrestPenalty;
                    log.Record("Guards", -defectors, Name);
                    if (state.Guards < MinGuardsThreshold)
                        log.Record("Unrest", LowGuardsUnrestPenalty, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new BetrayalFromWithinEvent();
    }
}
