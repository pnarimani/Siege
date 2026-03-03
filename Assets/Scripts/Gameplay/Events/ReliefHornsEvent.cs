using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefHornsEvent : GameEvent
    {
        const int DaysBeforeRelief = 3;

        public override string Id => "relief_horns";
        public override string Name => "War Horns Beyond the Hills";
        public override string Description => "War horns echo from beyond the hills.";
        public override int Priority => 95;

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;

        public override string GetNarrativeText(GameState state) =>
            "The unmistakable sound of war horns echoes from beyond the hills. " +
            "Someone is coming. Friend or foe, you cannot yet tell.";
    }
}
