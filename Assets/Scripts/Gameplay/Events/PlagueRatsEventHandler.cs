using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class PlagueRatsEventHandler : EventHandler<PlagueRatsEvent>
    {
        public PlagueRatsEventHandler(PlagueRatsEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 23;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Sickness += 10;
                    state.TotalDeaths += 2;
                    state.DeathsToday += 2;
                    state.Unrest += 5;
                    log.Record("Sickness", 10, Event.Name);
                    log.Record("TotalDeaths", 2, Event.Name);
                    log.Record("Unrest", 5, Event.Name);
                    break;
                case 1:
                    state.Sickness += 5;
                    state.AddResource(ResourceType.Materials, -10);
                    log.Record("Sickness", 5, Event.Name);
                    log.Record("Materials", -10, Event.Name);
                    break;
                case 2:
                    state.Sickness += 15;
                    state.TotalDeaths += 3;
                    state.DeathsToday += 3;
                    state.Unrest += 10;
                    log.Record("Sickness", 15, Event.Name);
                    log.Record("TotalDeaths", 3, Event.Name);
                    log.Record("Unrest", 10, Event.Name);
                    break;
            }
        }
    }
}
