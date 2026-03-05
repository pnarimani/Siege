using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class MilitiaVolunteersEvent : IGameEvent
    {
        const int TriggerDay = 6;
        const int MinWorkersRequired = 3;
        const int VolunteerCount = 3;
        const int VolunteerMoraleBoost = 3;
        const int ConscriptCount = 5;
        const int ConscriptUnrest = 5;

        bool _hasTriggered;

        public string Id => "militia_volunteers";
        public string Name => "Militia Volunteers";
        public string Description => "A group of workers offer to take up arms. Will you accept?";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay || state.HealthyWorkers < MinWorkersRequired)
                return false;

            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers -= VolunteerCount;
                    state.Guards += VolunteerCount;
                    log.Record("HealthyWorkers", -VolunteerCount, Name);
                    log.Record("Guards", VolunteerCount, Name);
                    break;
                case 1:
                    state.Morale += VolunteerMoraleBoost;
                    log.Record("Morale", VolunteerMoraleBoost, Name);
                    break;
                case 2:
                    state.HealthyWorkers -= ConscriptCount;
                    state.Guards += ConscriptCount;
                    state.Unrest += ConscriptUnrest;
                    log.Record("HealthyWorkers", -ConscriptCount, Name);
                    log.Record("Guards", ConscriptCount, Name);
                    log.Record("Unrest", ConscriptUnrest, Name);
                    break;
            }
        }

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept volunteers", "Workers -3, Guards +3"),
            new EventResponse("Decline", "Morale +3"),
            new EventResponse("Conscript more", "Workers -5, Guards +5, Unrest +5")
        };

        public IGameEvent Clone() => new MilitiaVolunteersEvent();
    }
}
