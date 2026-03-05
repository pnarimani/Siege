using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefHornsEvent : IGameEvent
    {
        const int DaysBeforeRelief = 3;

        readonly ReliefArmy _reliefArmy;
        bool _hasTriggered;

        public string Id => "relief_horns";
        public string Name => "War Horns Beyond the Hills";
        public string Description => "War horns echo from beyond the hills.";

        public ReliefHornsEvent(ReliefArmy reliefArmy)
        {
            _reliefArmy = reliefArmy;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (_reliefArmy.ArrivalDay <= 0 || state.CurrentDay != _reliefArmy.ArrivalDay - DaysBeforeRelief)
                return false;

            _hasTriggered = true;
            return true;
        }

        public string GetNarrativeText(GameState state) =>
            "The unmistakable sound of war horns echoes from beyond the hills. " +
            "Someone is coming. Friend or foe, you cannot yet tell.";

        public IGameEvent Clone() => new ReliefHornsEvent(_reliefArmy);
    }
}
