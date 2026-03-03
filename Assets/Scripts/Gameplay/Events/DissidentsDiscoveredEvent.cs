using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class DissidentsDiscoveredEvent : GameEvent
    {
        public override string Id => "dissidents_discovered";
        public override string Name => "Dissidents Discovered";
        public override string Description => "A ring of dissidents is uncovered plotting against the council. The city watches to see what you will do.";
        public override bool IsOneTime => true;
        public override int Priority => 55;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 10
                && Resolver.Resolve<PoliticalState>().Tyranny.Value >= 4
                && Random.value < 0.20f;
        }

        public override EventResponse[] GetResponses(GameState state)
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

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var p = Resolver.Resolve<PoliticalState>();

            switch (responseIndex)
            {
                case 0: // Execute
                    state.Unrest -= 15;
                    state.TotalDeaths += 3;
                    state.DeathsToday += 3;
                    state.Morale -= 5;
                    p.Tyranny.Add(1);
                    p.FearLevel.Add(1);
                    log.Record("Unrest", -15, Name);
                    log.Record("TotalDeaths", 3, Name);
                    log.Record("DeathsToday", 3, Name);
                    log.Record("Morale", -5, Name);
                    break;

                case 1: // Imprison
                    state.Unrest -= 10;
                    state.Morale += 5;
                    log.Record("Unrest", -10, Name);
                    log.Record("Morale", 5, Name);
                    break;

                case 2: // Release
                    state.Morale += 5;
                    state.Unrest += 8;
                    p.Faith.Add(1);
                    log.Record("Morale", 5, Name);
                    log.Record("Unrest", 8, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
