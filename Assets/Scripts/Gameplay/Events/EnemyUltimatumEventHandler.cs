using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyUltimatumEventHandler : IEventHandler
    {
        const int TriggerDay = 30;
        const int DefianceMoraleBoost = 10;
        const int DefianceUnrest = 15;
        const int PartialSurrenderMoralePenalty = 5;
        const int PartialSurrenderUnrest = 5;
        const int PartialSurrenderWorkerLoss = 2;
        const int FullSurrenderMoralePenalty = 15;
        const int FullSurrenderUnrest = 20;
        const int FullSurrenderWorkerLoss = 5;

        readonly EnemyUltimatumEvent _event;

        public string EventId => _event.Id;

        public EnemyUltimatumEventHandler(EnemyUltimatumEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) => state.CurrentDay == TriggerDay;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += DefianceMoraleBoost;
                    state.Unrest += DefianceUnrest;
                    log.Record("Morale", DefianceMoraleBoost, _event.Name);
                    log.Record("Unrest", DefianceUnrest, _event.Name);
                    break;
                case 1:
                    state.Morale -= PartialSurrenderMoralePenalty;
                    state.Unrest += PartialSurrenderUnrest;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - PartialSurrenderWorkerLoss);
                    log.Record("Morale", -PartialSurrenderMoralePenalty, _event.Name);
                    log.Record("Unrest", PartialSurrenderUnrest, _event.Name);
                    log.Record("HealthyWorkers", -PartialSurrenderWorkerLoss, _event.Name);
                    break;
                case 2:
                    state.Morale -= FullSurrenderMoralePenalty;
                    state.Unrest += FullSurrenderUnrest;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - FullSurrenderWorkerLoss);
                    log.Record("Morale", -FullSurrenderMoralePenalty, _event.Name);
                    log.Record("Unrest", FullSurrenderUnrest, _event.Name);
                    log.Record("HealthyWorkers", -FullSurrenderWorkerLoss, _event.Name);
                    break;
            }
        }
    }
}
