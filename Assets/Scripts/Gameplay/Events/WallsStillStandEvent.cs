using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallsStillStandEvent : GameEvent
    {
        const int StreakThreshold = 7;
        const double MoraleBoost = 8.0;

        int _lastFiredDay = int.MinValue;

        public override string Id => "walls_still_stand";
        public override string Name => "The Walls Still Stand";
        public override string Description => "The garrison's resolve inspires the city.";
        public override int Priority => 20;
        public override bool IsOneTime => false;

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveZoneHeldDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            state.Morale += MoraleBoost;
            log.Record("Morale", MoraleBoost, Name);
        }

        public override string GetNarrativeText(GameState state) =>
            "The walls hold firm. The garrison's determination inspires the whole city.";
    }
}
