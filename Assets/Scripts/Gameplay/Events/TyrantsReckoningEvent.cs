using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TyrantsReckoningEvent : IGameEvent
    {
        readonly PoliticalState _political;
        bool _hasTriggered;

        public string Id => "tyrants_reckoning";
        public string Name => "The Tyrant's Reckoning";
        public string Description => "The people have had enough. A delegation arrives at the council chamber — not asking, but demanding change.";

        public TyrantsReckoningEvent(PoliticalState political)
        {
            _political = political;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= 20 && _political.Tyranny.Value >= 8)
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
                    "Double down",
                    "+1 Tyranny, -30 Morale. Crush all dissent."),
                new EventResponse(
                    "Show mercy",
                    "-20 Unrest, +15 Morale, +2 Faith, -3 Tyranny. A rare olive branch.")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    _political.Tyranny.Add(1);
                    state.Morale -= 30;
                    log.Record("Morale", -30, Name);
                    break;

                case 1:
                    state.Unrest -= 20;
                    state.Morale += 15;
                    _political.Faith.Add(2);
                    _political.Tyranny.Add(-3);
                    log.Record("Unrest", -20, Name);
                    log.Record("Morale", 15, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new TyrantsReckoningEvent(_political);
    }
}
