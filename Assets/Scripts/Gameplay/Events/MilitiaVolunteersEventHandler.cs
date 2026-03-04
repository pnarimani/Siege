using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class MilitiaVolunteersEventHandler : IEventHandler
    {
        const int TriggerDay = 6;
        const int MinWorkersRequired = 3;
        const int VolunteerCount = 3;
        const int VolunteerMoraleBoost = 3;
        const int ConscriptCount = 5;
        const int ConscriptUnrest = 5;

        readonly MilitiaVolunteersEvent _event;

        public string EventId => _event.Id;

        public MilitiaVolunteersEventHandler(MilitiaVolunteersEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay == TriggerDay && state.HealthyWorkers >= MinWorkersRequired;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers -= VolunteerCount;
                    state.Guards += VolunteerCount;
                    log.Record("HealthyWorkers", -VolunteerCount, _event.Name);
                    log.Record("Guards", VolunteerCount, _event.Name);
                    break;
                case 1:
                    state.Morale += VolunteerMoraleBoost;
                    log.Record("Morale", VolunteerMoraleBoost, _event.Name);
                    break;
                case 2:
                    state.HealthyWorkers -= ConscriptCount;
                    state.Guards += ConscriptCount;
                    state.Unrest += ConscriptUnrest;
                    log.Record("HealthyWorkers", -ConscriptCount, _event.Name);
                    log.Record("Guards", ConscriptCount, _event.Name);
                    log.Record("Unrest", ConscriptUnrest, _event.Name);
                    break;
            }
        }
    }
}
