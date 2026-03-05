using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEvent : IGameEvent
    {
        const int DaysBeforeRelief = 1;

        readonly ReliefArmy _reliefArmy;
        bool _hasTriggered;

        public string Id => "relief_banners";
        public string Name => "Banners on the Ridge";
        public string Description => "Your kingdom's banners appear on the eastern ridge.";

        public ReliefBannersEvent(ReliefArmy reliefArmy)
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
            "Banners appear on the eastern ridge \u2014 your kingdom's colors. " +
            "The relief army is here. Hold one more day.";

        public IGameEvent Clone() => new ReliefBannersEvent(_reliefArmy);
    }
}
