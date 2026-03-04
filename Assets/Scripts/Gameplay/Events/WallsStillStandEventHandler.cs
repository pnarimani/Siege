using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallsStillStandEventHandler : EventHandler<WallsStillStandEvent>
    {
        const int StreakThreshold = 7;
        const double MoraleBoost = 8.0;

        int _lastFiredDay = int.MinValue;

        public WallsStillStandEventHandler(WallsStillStandEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveZoneHeldDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            log.Record("Morale", MoraleBoost, Event.Name);
        }
    }
}
