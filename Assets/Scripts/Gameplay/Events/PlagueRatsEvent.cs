using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class PlagueRatsEvent : IGameEvent
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

        bool _hasTriggered;

        public string Id => "plague_rats";
        public string Name => "Plague Rats";
        public string Description => "Rats swarm the lower districts, spreading disease. The people demand action.";

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Hunt the rats", "Sickness +10, Deaths +2, Unrest +5"),
            new EventResponse("Burn the infested quarter", "Sickness +5, Materials -10"),
            new EventResponse("Do nothing", "Sickness +15, Deaths +3, Unrest +10")
        };

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Sickness += QuarantineSickness;
                    state.TotalDeaths += QuarantineDeaths;
                    state.DeathsToday += QuarantineDeaths;
                    state.Unrest += QuarantineUnrest;
                    log.Record("Sickness", QuarantineSickness, Name);
                    log.Record("TotalDeaths", QuarantineDeaths, Name);
                    log.Record("Unrest", QuarantineUnrest, Name);
                    break;
                case 1:
                    state.Sickness += BurnSickness;
                    state.AddResource(ResourceType.Materials, -BurnMaterialCost);
                    log.Record("Sickness", BurnSickness, Name);
                    log.Record("Materials", -BurnMaterialCost, Name);
                    break;
                case 2:
                    state.Sickness += IgnoreSickness;
                    state.TotalDeaths += IgnoreDeaths;
                    state.DeathsToday += IgnoreDeaths;
                    state.Unrest += IgnoreUnrest;
                    log.Record("Sickness", IgnoreSickness, Name);
                    log.Record("TotalDeaths", IgnoreDeaths, Name);
                    log.Record("Unrest", IgnoreUnrest, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new PlagueRatsEvent();
    }
}
