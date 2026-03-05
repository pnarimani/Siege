using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DissidentsDiscoveredEvent : IGameEvent
    {
        readonly PoliticalState _political;
        bool _hasTriggered;

        public string Id => "dissidents_discovered";
        public string Name => "Dissidents Discovered";
        public string Description => "A ring of dissidents is uncovered plotting against the council. The city watches to see what you will do.";

        public DissidentsDiscoveredEvent(PoliticalState political)
        {
            _political = political;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= 10 && _political.Tyranny.Value >= 4 && Random.value < 0.20f)
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
                    "Execute them",
                    "-15 Unrest, +3 Deaths, -5 Morale, +1 Tyranny, +1 Fear"),
                new EventResponse(
                    "Imprison them",
                    "-10 Unrest, +5 Morale"),
                new EventResponse(
                    "Release them",
                    "+5 Morale, +8 Unrest, +1 Faith")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Unrest -= 15;
                    state.TotalDeaths += 3;
                    state.DeathsToday += 3;
                    state.Morale -= 5;
                    _political.Tyranny.Add(1);
                    _political.FearLevel.Add(1);
                    log.Record("Unrest", -15, Name);
                    log.Record("TotalDeaths", 3, Name);
                    log.Record("DeathsToday", 3, Name);
                    log.Record("Morale", -5, Name);
                    break;

                case 1:
                    state.Unrest -= 10;
                    state.Morale += 5;
                    log.Record("Unrest", -10, Name);
                    log.Record("Morale", 5, Name);
                    break;

                case 2:
                    state.Morale += 5;
                    state.Unrest += 8;
                    _political.Faith.Add(1);
                    log.Record("Morale", 5, Name);
                    log.Record("Unrest", 8, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new DissidentsDiscoveredEvent(_political);
    }
}
