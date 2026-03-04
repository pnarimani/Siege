using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class PlagueRatsEventHandler : IEventHandler
    {
        const int TriggerDay = 23;
        const int QuarantineSickness = 10;
        const int QuarantineDeaths = 2;
        const int QuarantineUnrest = 5;
        const int BurnSickness = 5;
        const int BurnMaterialCost = 10;
        const int IgnoreSickness = 15;
        const int IgnoreDeaths = 3;
        const int IgnoreUnrest = 10;

        readonly PlagueRatsEvent _event;

        public string EventId => _event.Id;

        public PlagueRatsEventHandler(PlagueRatsEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Sickness += QuarantineSickness;
                    state.TotalDeaths += QuarantineDeaths;
                    state.DeathsToday += QuarantineDeaths;
                    state.Unrest += QuarantineUnrest;
                    log.Record("Sickness", QuarantineSickness, _event.Name);
                    log.Record("TotalDeaths", QuarantineDeaths, _event.Name);
                    log.Record("Unrest", QuarantineUnrest, _event.Name);
                    break;
                case 1:
                    state.Sickness += BurnSickness;
                    state.AddResource(ResourceType.Materials, -BurnMaterialCost);
                    log.Record("Sickness", BurnSickness, _event.Name);
                    log.Record("Materials", -BurnMaterialCost, _event.Name);
                    break;
                case 2:
                    state.Sickness += IgnoreSickness;
                    state.TotalDeaths += IgnoreDeaths;
                    state.DeathsToday += IgnoreDeaths;
                    state.Unrest += IgnoreUnrest;
                    log.Record("Sickness", IgnoreSickness, _event.Name);
                    log.Record("TotalDeaths", IgnoreDeaths, _event.Name);
                    log.Record("Unrest", IgnoreUnrest, _event.Name);
                    break;
            }
        }
    }
}
