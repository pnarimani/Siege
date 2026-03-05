using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FortuneFavorsBoldEvent : IGameEvent
    {
        const int StreakThreshold = 3;
        const int VolunteerCount = 2;

        int _lastFiredDay = int.MinValue;

        public string Id => "fortune_favors_bold";
        public string Name => "Fortune Favors the Bold";
        public string Description => "Inspired by recent victories, 2 volunteers join the workforce.";

        public bool CanTrigger(GameState state)
        {
            if (state.ConsecutiveMissionSuccessDays >= StreakThreshold
                && state.CurrentDay != _lastFiredDay)
            {
                _lastFiredDay = state.CurrentDay;
                return true;
            }
            return false;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += VolunteerCount;
            log.Record("HealthyWorkers", VolunteerCount, Name);
        }

        public IGameEvent Clone() => new FortuneFavorsBoldEvent();
    }
}
