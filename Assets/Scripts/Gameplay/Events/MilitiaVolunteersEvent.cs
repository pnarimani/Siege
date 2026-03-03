using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class MilitiaVolunteersEvent : GameEvent
    {
        public override string Id => "militia_volunteers";
        public override string Name => "Militia Volunteers";
        public override string Description => "A group of workers offer to take up arms. Will you accept?";
        public override int Priority => 60;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay == 6 && state.HealthyWorkers >= 3;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept volunteers", "Workers -3, Guards +3"),
            new EventResponse("Decline", "Morale +3"),
            new EventResponse("Conscript more", "Workers -5, Guards +5, Unrest +5")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers -= 3;
                    state.Guards += 3;
                    log.Record("HealthyWorkers", -3, Name);
                    log.Record("Guards", 3, Name);
                    break;
                case 1:
                    state.Morale += 3;
                    log.Record("Morale", 3, Name);
                    break;
                case 2:
                    state.HealthyWorkers -= 5;
                    state.Guards += 5;
                    state.Unrest += 5;
                    log.Record("HealthyWorkers", -5, Name);
                    log.Record("Guards", 5, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }
    }
}
