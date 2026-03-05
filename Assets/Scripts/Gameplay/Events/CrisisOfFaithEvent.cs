using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CrisisOfFaithEvent : IGameEvent
    {
        readonly PoliticalState _political;
        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public string Id => "crisis_of_faith";
        public string Name => "Crisis of Faith";
        public string Description => "The faithful gather in the square, torn between devotion and despair. They look to you for guidance.";

        public CrisisOfFaithEvent(PoliticalState political, ResourceLedger ledger)
        {
            _political = political;
            _ledger = ledger;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= 15 && _political.Faith.Value >= 6 && state.Morale < 30)
            {
                _hasTriggered = true;
                return true;
            }
            return false;
        }

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Hold a vigil",
                    "+20 Morale, -10 Food, +5 Sickness, +1 Faith"),
                new EventResponse(
                    "Abandon the faith",
                    "-5 Morale, +10 Unrest, -3 Faith")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += 20;
                    _ledger.Withdraw(ResourceType.Food, 10);
                    state.Sickness += 5;
                    _political.Faith.Add(1);
                    log.Record("Morale", 20, Name);
                    log.Record("Food", -10, Name);
                    log.Record("Sickness", 5, Name);
                    break;

                case 1:
                    state.Morale -= 5;
                    state.Unrest += 10;
                    _political.Faith.Add(-3);
                    log.Record("Morale", -5, Name);
                    log.Record("Unrest", 10, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new CrisisOfFaithEvent(_political, _ledger);
    }
}
