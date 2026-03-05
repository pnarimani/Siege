using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyUltimatumEvent : IGameEvent
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

        bool _hasTriggered;

        public string Id => "enemy_ultimatum";
        public string Name => "Enemy Ultimatum";
        public string Description => "The enemy commander sends a final demand: surrender or face annihilation.";

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Defy them", "Morale +10, Unrest +15"),
            new EventResponse("Negotiate", "Morale -5, Unrest +5, Workers -2 (desertions)"),
            new EventResponse("Ignore", "Morale -15, Unrest +20, Workers -5 (desertions)")
        };

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay != TriggerDay) return false;
            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += DefianceMoraleBoost;
                    state.Unrest += DefianceUnrest;
                    log.Record("Morale", DefianceMoraleBoost, Name);
                    log.Record("Unrest", DefianceUnrest, Name);
                    break;
                case 1:
                    state.Morale -= PartialSurrenderMoralePenalty;
                    state.Unrest += PartialSurrenderUnrest;
                    state.HealthyWorkers = System.Math.Max(0, state.HealthyWorkers - PartialSurrenderWorkerLoss);
                    log.Record("Morale", -PartialSurrenderMoralePenalty, Name);
                    log.Record("Unrest", PartialSurrenderUnrest, Name);
                    log.Record("HealthyWorkers", -PartialSurrenderWorkerLoss, Name);
                    break;
                case 2:
                    state.Morale -= FullSurrenderMoralePenalty;
                    state.Unrest += FullSurrenderUnrest;
                    state.HealthyWorkers = System.Math.Max(0, state.HealthyWorkers - FullSurrenderWorkerLoss);
                    log.Record("Morale", -FullSurrenderMoralePenalty, Name);
                    log.Record("Unrest", FullSurrenderUnrest, Name);
                    log.Record("HealthyWorkers", -FullSurrenderWorkerLoss, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new EnemyUltimatumEvent();
    }
}
