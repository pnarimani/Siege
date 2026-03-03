using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEvent : GameEvent
    {
        const int DaysBeforeRelief = 1;

        public override string Id => "relief_banners";
        public override string Name => "Banners on the Ridge";
        public override string Description => "Your kingdom's banners appear on the eastern ridge.";
        public override int Priority => 95;

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;

        public override string GetNarrativeText(GameState state) =>
            "Banners appear on the eastern ridge \u2014 your kingdom's colors. " +
            "The relief army is here. Hold one more day.";
    }
}
