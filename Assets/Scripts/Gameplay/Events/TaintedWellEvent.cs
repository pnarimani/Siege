using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TaintedWellEvent : IGameEvent
    {
        const int TriggerDay = 18;
        const int WaterLoss = 20;
        const int SicknessIncrease = 10;

        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public TaintedWellEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "tainted_well";
        public string Name => "Tainted Well";
        public string Description => "The well-keeper reports a foul smell from the main cistern. By the time the warning spreads, many have already drunk.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Water, WaterLoss);
            log.Record("Water", -WaterLoss, Name);
            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Name);
        }

        public IGameEvent Clone() => new TaintedWellEvent(_ledger);
    }
}
