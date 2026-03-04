using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SteadySuppliesEventHandler : IEventHandler
    {
        readonly SteadySuppliesEvent _event;

        public string EventId => _event.Id;

        const int StreakThreshold = 5;
        const double MaxMoraleBoost = 15.0;

        int _lastFiredDay = int.MinValue;

        public SteadySuppliesEventHandler(SteadySuppliesEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.ConsecutiveNoDeficitDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            double boost = Math.Min(MaxMoraleBoost, state.ConsecutiveNoDeficitDays);
            state.Morale += boost;
            log.Record("Morale", boost, _event.Name);
        }
    }
}
