using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WellContaminationScareEventHandler : EventHandler<WellContaminationScareEvent>
    {
        public WellContaminationScareEventHandler(WellContaminationScareEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) => state.CurrentDay == 5;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    if (state.Medicine >= 5)
                    {
                        state.AddResource(ResourceType.Medicine, -5);
                        log.Record("Medicine", -5, Event.Name);
                    }
                    break;
                case 1:
                    state.Sickness += 1;
                    log.Record("Sickness", 1, Event.Name);
                    break;
                case 2:
                    state.Sickness += 5;
                    log.Record("Sickness", 5, Event.Name);
                    break;
            }
        }
    }
}
