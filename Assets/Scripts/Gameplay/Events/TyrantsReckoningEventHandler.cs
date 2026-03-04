using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TyrantsReckoningEventHandler : EventHandler<TyrantsReckoningEvent>
    {
        readonly PoliticalState _political;

        public TyrantsReckoningEventHandler(TyrantsReckoningEvent gameEvent, PoliticalState political) : base(gameEvent)
        {
            _political = political;
        }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 20 && _political.Tyranny.Value >= 8;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    _political.Tyranny.Add(1);
                    state.Morale -= 30;
                    log.Record("Morale", -30, Event.Name);
                    break;

                case 1:
                    state.Unrest -= 20;
                    state.Morale += 15;
                    _political.Faith.Add(2);
                    _political.Tyranny.Add(-3);
                    log.Record("Unrest", -20, Event.Name);
                    log.Record("Morale", 15, Event.Name);
                    break;
            }
        }
    }
}
