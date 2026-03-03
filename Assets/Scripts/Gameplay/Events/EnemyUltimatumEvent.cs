using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EnemyUltimatumEvent : GameEvent
    {
        public override string Id => "enemy_ultimatum";
        public override string Name => "Enemy Ultimatum";
        public override string Description => "The enemy commander sends a final demand: surrender or face annihilation.";
        public override int Priority => 80;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 30;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Defy them", "Morale +10, Unrest +15"),
            new EventResponse("Negotiate", "Morale -5, Unrest +5, Workers -2 (desertions)"),
            new EventResponse("Ignore", "Morale -15, Unrest +20, Workers -5 (desertions)")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.Morale += 10;
                    state.Unrest += 15;
                    log.Record("Morale", 10, Name);
                    log.Record("Unrest", 15, Name);
                    break;
                case 1:
                    state.Morale -= 5;
                    state.Unrest += 5;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 2);
                    log.Record("Morale", -5, Name);
                    log.Record("Unrest", 5, Name);
                    log.Record("HealthyWorkers", -2, Name);
                    break;
                case 2:
                    state.Morale -= 15;
                    state.Unrest += 20;
                    state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 5);
                    log.Record("Morale", -15, Name);
                    log.Record("Unrest", 20, Name);
                    log.Record("HealthyWorkers", -5, Name);
                    break;
            }
        }
    }
}
