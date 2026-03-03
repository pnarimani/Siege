using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallBreachEvent : GameEvent
    {
        public override string Id => "wall_breach";
        public override string Name => "Wall Breach";
        public override string Description => "The perimeter wall groans and splits. The enemy will not wait long to exploit the gap.";
        public override bool IsOneTime => false;
        public override int Priority => 85;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.Zones[state.ActivePerimeter].Integrity < 30;
        }

        public override EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Reinforce the breach",
                    state.Guards >= 15
                        ? "Guards hold the line — breach contained."
                        : "Not enough guards — the wall buckles further."),
                new EventResponse(
                    "Barricade with materials",
                    "-10 Materials, minor integrity loss."),
                new EventResponse(
                    "Fall back",
                    "Abandon the section — significant integrity loss.")
            };
        }

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var zone = state.ActivePerimeter;

            switch (responseIndex)
            {
                case 0: // Reinforce
                    if (state.Guards >= 15)
                    {
                        log.Record("ZoneIntegrity:" + zone, 0, Name + " (held)");
                    }
                    else
                    {
                        state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 8);
                        log.Record("ZoneIntegrity:" + zone, -8, Name);
                    }
                    break;

                case 1: // Barricade
                    state.Materials = Math.Max(0, state.Materials - 10);
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 5);
                    log.Record("Materials", -10, Name);
                    log.Record("ZoneIntegrity:" + zone, -5, Name);
                    break;

                case 2: // Fall back
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 15);
                    log.Record("ZoneIntegrity:" + zone, -15, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
