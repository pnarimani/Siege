using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class FortuneFavorsBoldEvent : GameEvent
    {
        const int StreakThreshold = 3;
        const int VolunteerCount = 2;

        int _lastFiredDay = int.MinValue;

        public override string Id => "fortune_favors_bold";
        public override string Name => "Fortune Favors the Bold";
        public override string Description => "Recent victories inspire volunteers.";
        public override int Priority => 20;
        public override bool IsOneTime => false;

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveMissionSuccessDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.HealthyWorkers += VolunteerCount;
            log.Record("HealthyWorkers", VolunteerCount, Name);
        }

        public override string GetNarrativeText(GameState state) =>
            "Inspired by recent victories, 2 volunteers join the workforce.";
    }
}
