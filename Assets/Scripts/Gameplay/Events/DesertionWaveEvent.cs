using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class DesertionWaveEvent : IGameEvent
    {
        const int MoraleThreshold = 30;
        const int MaxWorkersLost = 10;

        bool _hasTriggered;

        public string Id => "desertion_wave";
        public string Name => "Desertion Wave";
        public string Description => "At dawn, the western gate stands open. Footprints in the mud lead away into the fog.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.Morale >= MoraleThreshold) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            int lost = System.Math.Min(MaxWorkersLost, state.HealthyWorkers);
            state.HealthyWorkers -= lost;
            log.Record("HealthyWorkers", -lost, Name);
        }

        public IGameEvent Clone() => new DesertionWaveEvent();
    }
}
