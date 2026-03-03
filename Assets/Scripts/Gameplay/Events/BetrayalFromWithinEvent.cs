using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class BetrayalFromWithinEvent : GameEvent
    {
        public override string Id => "betrayal_from_within";
        public override string Name => "Betrayal from Within";
        public override string Description => "A faction of guards has been meeting in secret. They plan to open the gates at dawn. You learn of it just in time.";
        public override bool IsOneTime => true;
        public override int Priority => 80;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay == 37;
        }

        public override EventResponse[] GetResponses(GameState state)
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

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            int defectors = state.Guards / 3;

            switch (responseIndex)
            {
                case 0: // Amnesty
                    state.Guards -= defectors;
                    state.HealthyWorkers += defectors;
                    state.Morale += 5;
                    log.Record("Guards", -defectors, Name);
                    log.Record("HealthyWorkers", defectors, Name);
                    log.Record("Morale", 5, Name);
                    break;

                case 1: // Make example
                    int executed = Math.Min(2, defectors);
                    int converted = Math.Max(0, defectors - 2);
                    state.TotalDeaths += executed;
                    state.DeathsToday += executed;
                    state.Guards -= defectors;
                    state.HealthyWorkers += converted;
                    state.Unrest += 10;
                    log.Record("TotalDeaths", executed, Name);
                    log.Record("DeathsToday", executed, Name);
                    log.Record("Guards", -defectors, Name);
                    log.Record("HealthyWorkers", converted, Name);
                    log.Record("Unrest", 10, Name);
                    break;

                case 2: // Let them go
                    state.Guards -= defectors;
                    if (state.Guards < 5)
                        state.Unrest += 15;
                    log.Record("Guards", -defectors, Name);
                    if (state.Guards < 5)
                        log.Record("Unrest", 15, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
