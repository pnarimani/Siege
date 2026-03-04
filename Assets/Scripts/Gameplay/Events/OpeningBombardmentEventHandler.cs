using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class OpeningBombardmentEventHandler : IEventHandler
    {
        const int TriggerDay = 1;
        const int IntegrityDamage = 10;
        const int FoodLoss = 10;

        readonly OpeningBombardmentEvent _event;

        public string EventId => _event.Id;

        public OpeningBombardmentEventHandler(OpeningBombardmentEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void Execute(GameState state, ChangeLog log)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.Integrity -= IntegrityDamage;
            log.Record("OuterFarms.Integrity", -IntegrityDamage, _event.Name);
            state.AddResource(ResourceType.Food, -FoodLoss);
            log.Record("Food", -FoodLoss, _event.Name);
        }
    }
}
