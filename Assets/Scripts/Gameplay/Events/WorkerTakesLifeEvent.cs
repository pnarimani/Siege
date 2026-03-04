using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WorkerTakesLifeEvent : IGameEvent
    {
        public bool HasTriggered { get; set; }
        public string Id => "worker_takes_life";
        public string Name => "A Quiet Departure";
        public string Description => "A body is found in the quiet hours. No enemy arrow, no illness — just a soul that could bear no more.";
        public bool IsOneTime => true;
        public int Priority => 60;

        public string GetNarrativeText(GameState state) => Description;
    }
}
