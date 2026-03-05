using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SiegeEngineersArriveEvent : IGameEvent
    {
        readonly PoliticalState _political;
        bool _hasTriggered;

        public string Id => "siege_engineers_arrive";
        public string Name => "Siege Engineers Arrive";
        public string Description => "A small band of military engineers approaches the gate, offering their skills in exchange for shelter and rations.";

        public SiegeEngineersArriveEvent(PoliticalState political)
        {
            _political = political;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= 15 && _political.Fortification.Value >= 5 && Random.value < 0.25f)
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
                    "Accept them",
                    "+3 Workers, +20 Materials, -10 Food, +1 Fortification"),
                new EventResponse(
                    "Decline",
                    "+5 Morale")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers += 3;
                    state.Materials += 20;
                    state.Food = System.Math.Max(0, state.Food - 10);
                    _political.Fortification.Add(1);
                    log.Record("HealthyWorkers", 3, Name);
                    log.Record("Materials", 20, Name);
                    log.Record("Food", -10, Name);
                    break;

                case 1:
                    state.Morale += 5;
                    log.Record("Morale", 5, Name);
                    break;
            }
        }

        public string GetNarrativeText(GameState state) => Description;

        public IGameEvent Clone() => new SiegeEngineersArriveEvent(_political);
    }
}
