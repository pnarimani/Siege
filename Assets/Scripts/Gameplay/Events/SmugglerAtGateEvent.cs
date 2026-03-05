using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SmugglerAtGateEvent : IGameEvent
    {
        bool _hasTriggered;

        public string Id => "smuggler_at_gate";
        public string Name => "Smuggler at the Gate";
        public string Description => "A smuggler offers goods at the gate. His prices are steep, but the city is hungry.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 3) return false;

            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.AddResource(ResourceType.Food, 20);
                    state.AddResource(ResourceType.Materials, -15);
                    log.Record("Food", 20, Name);
                    log.Record("Materials", -15, Name);
                    break;
                case 1:
                    state.AddResource(ResourceType.Food, 30);
                    state.AddResource(ResourceType.Materials, -15);
                    state.Unrest += 5;
                    log.Record("Food", 30, Name);
                    log.Record("Materials", -15, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept the deal", "Food +20, Materials -15"),
            new EventResponse("Demand a better deal", "Food +30, Materials -15, Unrest +5"),
            new EventResponse("Turn him away", "No trade")
        };

        public string GetNarrativeText(GameState state) =>
            "A cloaked figure at the postern gate. He has food—but wants materials in return. Do you deal?";

        public IGameEvent Clone() => new SmugglerAtGateEvent();
    }
}
