using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WellContaminationScareEvent : GameEvent
    {
        public override string Id => "well_contamination_scare";
        public override string Name => "Well Contamination Scare";
        public override string Description => "Rumours spread that the main well may be tainted. The people look to you for a decision.";
        public override int Priority => 60;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 5;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Treat with medicine",
                state.Medicine >= 5 ? "Medicine -5" : "(Requires 5 Medicine)"),
            new EventResponse("Boil all water", "Sickness +1"),
            new EventResponse("Ignore the rumours", "Sickness +5")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
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
    }
}
