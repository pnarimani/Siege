namespace Siege.Gameplay.Events
{
    public class FinalAssaultEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "final_assault";
        public string Name => "Final Assault";
        public string Description => "Battering rams thunder against the gates. The enemy throws everything at the walls. This is the final assault.";
        public int Priority => 100;
    }
}
