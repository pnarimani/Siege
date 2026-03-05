using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class OpeningBombardmentEvent : IGameEvent
    {
        const int TriggerDay = 1;
        const int IntegrityDamage = 10;
        const int FoodLoss = 10;

        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public OpeningBombardmentEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "opening_bombardment";
        public string Name => "Opening Bombardment";
        public string Description => "The first boulders crash into Outer Farms. Smoke rises from burning granaries as the enemy demonstrates their intent.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            var zone = state.Zones[ZoneId.OuterFarms];
            zone.Integrity -= IntegrityDamage;
            log.Record("OuterFarms.Integrity", -IntegrityDamage, Name);
            _ledger.Withdraw(ResourceType.Food, FoodLoss);
            log.Record("Food", -FoodLoss, Name);
        }

        public IGameEvent Clone() => new OpeningBombardmentEvent(_ledger);
    }
}
