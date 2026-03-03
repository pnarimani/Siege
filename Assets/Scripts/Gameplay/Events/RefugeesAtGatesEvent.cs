using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class RefugeesAtGatesEvent : GameEvent
    {
        public override string Id => "refugees_at_gates";
        public override string Name => "Refugees at the Gates";
        public override string Description => "A crowd of refugees from the countryside begs entry. Some look sick.";
        public override int Priority => 60;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) => state.CurrentDay == 12;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Open the gates", "Workers +5, Sick +3, Unrest +5, Morale +3"),
            new EventResponse("Admit only the healthy", "Workers +5, Unrest +3"),
            new EventResponse("Turn them away", "Morale -10, Unrest +5")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    state.HealthyWorkers += 5;
                    state.SickWorkers += 3;
                    state.Unrest += 5;
                    state.Morale += 3;
                    log.Record("HealthyWorkers", 5, Name);
                    log.Record("SickWorkers", 3, Name);
                    log.Record("Unrest", 5, Name);
                    log.Record("Morale", 3, Name);
                    break;
                case 1:
                    state.HealthyWorkers += 5;
                    state.Unrest += 3;
                    log.Record("HealthyWorkers", 5, Name);
                    log.Record("Unrest", 3, Name);
                    break;
                case 2:
                    state.Morale -= 10;
                    state.Unrest += 5;
                    log.Record("Morale", -10, Name);
                    log.Record("Unrest", 5, Name);
                    break;
            }
        }
    }
}
