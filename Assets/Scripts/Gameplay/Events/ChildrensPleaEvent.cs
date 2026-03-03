using System;
using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class ChildrensPleaEvent : GameEvent
    {
        public override string Id => "childrens_plea";
        public override string Name => "Children's Plea";
        public override string Description => "A group of orphaned children approaches the council, begging for shelter and food.";
        public override bool IsOneTime => true;
        public override int Priority => 50;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 12
                && Resolver.Resolve<PoliticalState>().Faith.Value >= 3
                && UnityEngine.Random.value < 0.15f;
        }

        public override EventResponse[] GetResponses(GameState state)
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

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var p = Resolver.Resolve<PoliticalState>();

            switch (responseIndex)
            {
                case 0: // Grant shelter
                    state.Materials = Math.Max(0, state.Materials - 10);
                    state.Morale += 10;
                    state.Sickness += 3;
                    p.Faith.Add(1);
                    log.Record("Materials", -10, Name);
                    log.Record("Morale", 10, Name);
                    log.Record("Sickness", 3, Name);
                    break;

                case 1: // Refuse
                    state.Morale -= 5;
                    state.Unrest += 5;
                    p.Tyranny.Add(1);
                    log.Record("Morale", -5, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
