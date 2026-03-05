using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class ChildrensPleaEvent : IGameEvent
    {
        readonly PoliticalState _political;
        bool _hasTriggered;

        public string Id => "childrens_plea";
        public string Name => "Children's Plea";
        public string Description => "A group of orphaned children approaches the council, begging for shelter and food.";

        public ChildrensPleaEvent(PoliticalState political)
        {
            _political = political;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= 12 && _political.Faith.Value >= 3 && Random.value < 0.15f)
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
                    "Grant them shelter",
                    "-10 Materials, +10 Morale, +3 Sickness, +1 Faith"),
                new EventResponse(
                    "Refuse them",
                    "-5 Morale, +5 Unrest, +1 Tyranny")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Materials = System.Math.Max(0, state.Materials - 10);
                    state.Morale += 10;
                    state.Sickness += 3;
                    _political.Faith.Add(1);
                    log.Record("Materials", -10, Name);
                    log.Record("Morale", 10, Name);
                    log.Record("Sickness", 3, Name);
                    break;

                case 1:
                    state.Morale -= 5;
                    state.Unrest += 5;
                    _political.Tyranny.Add(1);
                    log.Record("Morale", -5, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }

        public string GetNarrativeText(GameState state) => Description;

        public IGameEvent Clone() => new ChildrensPleaEvent(_political);
    }
}
