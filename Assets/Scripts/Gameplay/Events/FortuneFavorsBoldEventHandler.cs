using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FortuneFavorsBoldEventHandler : EventHandler<FortuneFavorsBoldEvent>
    {
        const int StreakThreshold = 3;
        const int VolunteerCount = 2;

        int _lastFiredDay = int.MinValue;

        public FortuneFavorsBoldEventHandler(FortuneFavorsBoldEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveMissionSuccessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.HealthyWorkers += VolunteerCount;
            log.Record("HealthyWorkers", VolunteerCount, Event.Name);
        }
    }
}
