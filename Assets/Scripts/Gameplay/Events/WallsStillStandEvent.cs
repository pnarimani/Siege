using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallsStillStandEvent : IGameEvent
    {
        const int StreakThreshold = 7;
        const double MoraleBoost = 8.0;

        int _lastTriggerDay = int.MinValue;

        public string Id => "walls_still_stand";
        public string Name => "The Walls Still Stand";
        public string Description => "The walls hold firm. The garrison's determination inspires the whole city.";

        public bool CanTrigger(GameState state)
        {
            if (state.ConsecutiveZoneHeldDays < StreakThreshold || state.CurrentDay == _lastTriggerDay)
                return false;

            _lastTriggerDay = state.CurrentDay;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.Morale += MoraleBoost;
            log.Record("Morale", MoraleBoost, Name);
        }

        public IGameEvent Clone() => new WallsStillStandEvent();
    }
}
