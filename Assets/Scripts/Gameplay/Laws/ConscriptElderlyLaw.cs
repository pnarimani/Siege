using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class ConscriptElderlyLaw : Law
    {
        const double MoraleCost = -20;
        const double UnrestIncrease = 10;
        const int DailyDeaths = 1;

        public override string Id => "conscript_elderly";
        public override string Name => "Conscript the Elderly";
        public override string Description => "Draft the elderly into the workforce. They will work, but they will not last.";
        public override string NarrativeText => "Grandfather picked up a shovel today. He did not put it down.";

        public override bool CanEnact(GameState state) => state.Elderly > 0;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int converted = state.Elderly;
            state.HealthyWorkers += converted;
            state.Elderly = 0;
            log.Record("HealthyWorkers", converted, "Conscript Elderly");
            log.Record("Elderly", -converted, "Conscript Elderly");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Conscript Elderly");

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, "Conscript Elderly");
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            if (state.HealthyWorkers <= 0) return;

            state.HealthyWorkers -= DailyDeaths;
            state.TotalDeaths += DailyDeaths;
            state.DeathsToday += DailyDeaths;
            log.Record("HealthyWorkers", -DailyDeaths, "Conscript Elderly (attrition)");
        }
    }
}
