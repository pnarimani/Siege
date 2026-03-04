using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class RefugeesAtGatesEvent : GameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "refugees_at_gates";
        public string Name => "Refugees at the Gates";
        public string Description => "A crowd of refugees from the countryside begs entry. Some look sick.";
        public int Priority => 60;
        public bool IsRespondable => true;

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Open the gates", "Workers +5, Sick +3, Unrest +5, Morale +3"),
            new EventResponse("Admit only the healthy", "Workers +5, Unrest +3"),
            new EventResponse("Turn them away", "Morale -10, Unrest +5")
        };

    }
}
