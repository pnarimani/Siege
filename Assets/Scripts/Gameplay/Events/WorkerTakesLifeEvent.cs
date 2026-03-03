using System;
using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WorkerTakesLifeEvent : GameEvent
    {
        public override string Id => "worker_takes_life";
        public override string Name => "A Quiet Departure";
        public override string Description => "A body is found in the quiet hours. No enemy arrow, no illness — just a soul that could bear no more.";
        public override bool IsOneTime => true;
        public override int Priority => 60;

        public override bool CanTrigger(GameState state)
        {
            var p = Resolver.Resolve<PoliticalState>();
            return p.Humanity.Value < 15 && state.Morale < 30;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            state.TotalDeaths += 1;
            state.DeathsToday += 1;
            state.HealthyWorkers = Math.Max(0, state.HealthyWorkers - 1);
            state.Morale -= 5;

            log.Record("TotalDeaths", 1, Name);
            log.Record("DeathsToday", 1, Name);
            log.Record("HealthyWorkers", -1, Name);
            log.Record("Morale", -5, Name);
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
