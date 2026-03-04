using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SignalFireEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "signal_fire";
        public string Name => "Light the Signal Fire";
        public string Description => "The beacon tower still stands. If we light the fires, someone beyond the hills may see — but so will the enemy.";
        public bool IsOneTime => true;
        public int Priority => 70;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Light the fires",
                    "-5 Fuel, -15 Materials, +5 Unrest. The signal burns."),
                new EventResponse(
                    "Too risky",
                    "The beacon remains dark.")
            };
        }

        public string GetNarrativeText(GameState state) => Description;
    }
}
