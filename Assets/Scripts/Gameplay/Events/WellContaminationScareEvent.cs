using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WellContaminationScareEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "well_contamination_scare";
        public string Name => "Well Contamination Scare";
        public string Description => "Rumours spread that the main well may be tainted. The people look to you for a decision.";

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Treat with medicine",
                state.Medicine >= 5 ? "Medicine -5" : "(Requires 5 Medicine)"),
            new EventResponse("Boil all water", "Sickness +1"),
            new EventResponse("Ignore the rumours", "Sickness +5")
        };

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 5) return false;
            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    if (state.Medicine >= 5)
                    {
                        state.AddResource(ResourceType.Medicine, -5);
                        log.Record("Medicine", -5, Name);
                    }
                    break;
                case 1:
                    state.Sickness += 1;
                    log.Record("Sickness", 1, Name);
                    break;
                case 2:
                    state.Sickness += 5;
                    log.Record("Sickness", 5, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new WellContaminationScareEvent();
    }
}
