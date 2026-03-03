using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class SacrificeSickOrder : Order
    {
        const int Cooldown = 3;
        const int SickRemoved = 3;
        const double SicknessReduction = 8;
        const double UnrestIncrease = 12;
        const double MoraleLoss = 10;
        const int MinSick = 5;

        public override string Id => "sacrifice_sick";
        public override string Name => "Sacrifice the Sick";
        public override string Description => "Remove the terminally ill to slow the spread of disease. A decision no one will forget.";
        public override string NarrativeText => "They are taken beyond the walls at night. No one speaks of it in the morning.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.SickWorkers > MinSick && Resolver.Resolve<PoliticalState>().Tyranny.Value >= 3;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.SickWorkers -= SickRemoved;
            log.Record("SickWorkers", -SickRemoved, Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);

            state.TotalDeaths += SickRemoved;
            state.DeathsToday += SickRemoved;
            log.Record("Deaths", SickRemoved, Id);
        }
    }
}
