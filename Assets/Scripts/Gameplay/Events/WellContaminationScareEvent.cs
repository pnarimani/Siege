using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WellContaminationScareEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "well_contamination_scare";
        public string Name => "Well Contamination Scare";
        public string Description => "Rumours spread that the main well may be tainted. The people look to you for a decision.";
        public int Priority => 60;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Treat with medicine",
                state.Medicine >= 5 ? "Medicine -5" : "(Requires 5 Medicine)"),
            new EventResponse("Boil all water", "Sickness +1"),
            new EventResponse("Ignore the rumours", "Sickness +5")
        };

    }
}
