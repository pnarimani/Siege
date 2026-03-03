using System;
using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class CrisisOfFaithEvent : GameEvent
    {
        public override string Id => "crisis_of_faith";
        public override string Name => "Crisis of Faith";
        public override string Description => "The faithful gather in the square, torn between devotion and despair. They look to you for guidance.";
        public override bool IsOneTime => true;
        public override int Priority => 60;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 15
                && Resolver.Resolve<PoliticalState>().Faith.Value >= 6
                && state.Morale < 30;
        }

        public override EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Hold a vigil",
                    "+20 Morale, -10 Food, +5 Sickness, +1 Faith"),
                new EventResponse(
                    "Abandon the faith",
                    "-5 Morale, +10 Unrest, -3 Faith")
            };
        }

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var p = Resolver.Resolve<PoliticalState>();

            switch (responseIndex)
            {
                case 0: // Hold Vigil
                    state.Morale += 20;
                    state.Food = Math.Max(0, state.Food - 10);
                    state.Sickness += 5;
                    p.Faith.Add(1);
                    log.Record("Morale", 20, Name);
                    log.Record("Food", -10, Name);
                    log.Record("Sickness", 5, Name);
                    break;

                case 1: // Abandon Faith
                    state.Morale -= 5;
                    state.Unrest += 10;
                    p.Faith.Add(-3);
                    log.Record("Morale", -5, Name);
                    log.Record("Unrest", 10, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
