using System;
using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SiegeEngineersArriveEvent : GameEvent
    {
        public override string Id => "siege_engineers_arrive";
        public override string Name => "Siege Engineers Arrive";
        public override string Description => "A small band of military engineers approaches the gate, offering their skills in exchange for shelter and rations.";
        public override bool IsOneTime => true;
        public override int Priority => 50;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 15
                && Resolver.Resolve<PoliticalState>().Fortification.Value >= 5
                && UnityEngine.Random.value < 0.25f;
        }

        public override EventResponse[] GetResponses(GameState state)
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

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var p = Resolver.Resolve<PoliticalState>();

            switch (responseIndex)
            {
                case 0: // Accept
                    state.HealthyWorkers += 3;
                    state.Materials += 20;
                    state.Food = Math.Max(0, state.Food - 10);
                    p.Fortification.Add(1);
                    log.Record("HealthyWorkers", 3, Name);
                    log.Record("Materials", 20, Name);
                    log.Record("Food", -10, Name);
                    break;

                case 1: // Decline
                    state.Morale += 5;
                    log.Record("Morale", 5, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
