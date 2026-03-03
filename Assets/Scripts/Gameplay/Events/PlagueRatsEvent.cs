using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class PlagueRatsEvent : GameEvent
    {
        public override string Id => "plague_rats";
        public override string Name => "Plague Rats";
        public override string Description => "Rats swarm the lower districts, spreading disease. The people demand action.";
        public override int Priority => 70;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 23;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Hunt the rats", "Sickness +10, Deaths +2, Unrest +5"),
            new EventResponse("Burn the infested quarter", "Sickness +5, Materials -10"),
            new EventResponse("Do nothing", "Sickness +15, Deaths +3, Unrest +10")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Sickness += 10;
                    state.TotalDeaths += 2;
                    state.DeathsToday += 2;
                    state.Unrest += 5;
                    log.Record("Sickness", 10, Name);
                    log.Record("TotalDeaths", 2, Name);
                    log.Record("Unrest", 5, Name);
                    break;
                case 1:
                    state.Sickness += 5;
                    state.AddResource(ResourceType.Materials, -10);
                    log.Record("Sickness", 5, Name);
                    log.Record("Materials", -10, Name);
                    break;
                case 2:
                    state.Sickness += 15;
                    state.TotalDeaths += 3;
                    state.DeathsToday += 3;
                    state.Unrest += 10;
                    log.Record("Sickness", 15, Name);
                    log.Record("TotalDeaths", 3, Name);
                    log.Record("Unrest", 10, Name);
                    break;
            }
        }
    }
}
