using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SignalFireEvent : GameEvent
    {
        public override string Id => "signal_fire";
        public override string Name => "Light the Signal Fire";
        public override string Description => "The beacon tower still stands. If we light the fires, someone beyond the hills may see — but so will the enemy.";
        public override bool IsOneTime => true;
        public override int Priority => 70;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 25
                && state.Zones[ZoneId.Keep].Integrity >= 30
                && !state.SignalFireLit
                && state.Fuel >= 5
                && state.Materials >= 15;
        }

        public override EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Light the fires",
                    "-5 Fuel, -15 Materials, +5 Unrest. The signal burns."),
                new EventResponse(
                    "Too risky",
                    "The beacon remains dark.")
            };
        }

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0: // Light fires
                    state.Fuel = Math.Max(0, state.Fuel - 5);
                    state.Materials = Math.Max(0, state.Materials - 15);
                    state.Unrest += 5;
                    state.SignalFireLit = true;
                    log.Record("Fuel", -5, Name);
                    log.Record("Materials", -15, Name);
                    log.Record("Unrest", 5, Name);
                    break;

                case 1: // Too risky
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
