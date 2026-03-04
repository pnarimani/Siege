using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class MilitiaVolunteersEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "militia_volunteers";
        public string Name => "Militia Volunteers";
        public string Description => "A group of workers offer to take up arms. Will you accept?";
        public int Priority => 60;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept volunteers", "Workers -3, Guards +3"),
            new EventResponse("Decline", "Morale +3"),
            new EventResponse("Conscript more", "Workers -5, Guards +5, Unrest +5")
        };

    }
}
