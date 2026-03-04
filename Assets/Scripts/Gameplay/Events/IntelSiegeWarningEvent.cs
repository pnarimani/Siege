using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class IntelSiegeWarningEvent : GameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "intel_siege_warning";
        public string Name => "Siege Escalation Warning";
        public string Description => "Intelligence warns of an imminent siege escalation.";
        public int Priority => 55;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Intercept",
                "Send guards to intercept. Some will not return, but siege pressure eases.",
                "Your guards ambush the enemy's forward scouts under cover of night. " +
                "Not all return, but the enemy's advance falters."),
            new EventResponse(
                "Brace",
                "Reinforce the perimeter instead.",
                "Work crews shore up the walls through the night. " +
                "The perimeter holds a little stronger.")
        };

        public string GetNarrativeText(GameState state) =>
            "A captured messenger reveals plans for a major assault. " +
            "You have a narrow window to act.";
    }
}
