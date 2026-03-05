using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SmugglerAtGateEvent : IGameEvent
    {
        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public SmugglerAtGateEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "smuggler_at_gate";
        public string Name => "Smuggler at the Gate";
        public string Description => "A cloaked figure at the postern gate. He has food\u2014but wants materials in return. Do you deal?";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != 3) return false;

            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    _ledger.Deposit(ResourceType.Food, 20);
                    _ledger.Withdraw(ResourceType.Materials, 15);
                    log.Record("Food", 20, Name);
                    log.Record("Materials", -15, Name);
                    break;
                case 1:
                    _ledger.Deposit(ResourceType.Food, 30);
                    _ledger.Withdraw(ResourceType.Materials, 15);
                    state.Unrest += 5;
                    log.Record("Food", 30, Name);
                    log.Record("Materials", -15, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept the deal", "Food +20, Materials -15"),
            new EventResponse("Demand a better deal", "Food +30, Materials -15, Unrest +5"),
            new EventResponse("Turn him away", "No trade")
        };

        public IGameEvent Clone() => new SmugglerAtGateEvent(_ledger);
    }
}
