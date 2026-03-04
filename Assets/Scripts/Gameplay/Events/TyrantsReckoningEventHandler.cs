using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TyrantsReckoningEventHandler : IEventHandler
    {
        readonly TyrantsReckoningEvent _event;

        public string EventId => _event.Id;

        readonly PoliticalState _political;

        public TyrantsReckoningEventHandler(TyrantsReckoningEvent gameEvent, PoliticalState political)
        {
            _event = gameEvent;
            _political = political;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 20 && _political.Tyranny.Value >= 8;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    _political.Tyranny.Add(1);
                    state.Morale -= 30;
                    log.Record("Morale", -30, _event.Name);
                    break;

                case 1:
                    state.Unrest -= 20;
                    state.Morale += 15;
                    _political.Faith.Add(2);
                    _political.Tyranny.Add(-3);
                    log.Record("Unrest", -20, _event.Name);
                    log.Record("Morale", 15, _event.Name);
                    break;
            }
        }
    }
}
