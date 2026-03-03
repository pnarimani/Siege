using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class PublicTrialOrder : Order
    {
        const int Cooldown = 5;
        const int Deaths = 2;
        const double UnrestReduction = 5;
        const double MoraleLoss = 10;

        public override string Id => "public_trial";
        public override string Name => "Public Trial";
        public override string Description => "Hold a public trial to make an example of dissenters.";
        public override string NarrativeText => "The verdict was decided before the trial began. Everyone knows it. No one objects.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state)
        {
            var p = Resolver.Resolve<PoliticalState>();
            return p.Tyranny.Value >= 2 || p.Faith.Value >= 2;
        }

        public override void Execute(GameState state, ChangeLog log)
        {
            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);

            // Tyranny path default
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);
        }
    }
}
