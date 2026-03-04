using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FortuneFavorsBoldEventHandler : IEventHandler
    {
        readonly FortuneFavorsBoldEvent _event;

        public string EventId => _event.Id;

        const int StreakThreshold = 3;
        const int VolunteerCount = 2;

        int _lastFiredDay = int.MinValue;

        public FortuneFavorsBoldEventHandler(FortuneFavorsBoldEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveMissionSuccessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.HealthyWorkers += VolunteerCount;
            log.Record("HealthyWorkers", VolunteerCount, _event.Name);
        }
    }
}
