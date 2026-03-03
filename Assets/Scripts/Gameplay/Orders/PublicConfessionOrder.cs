using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class PublicConfessionOrder : Order
    {
        const int Cooldown = 3;
        const double UnrestReduction = 20;
        const double MoraleLoss = 10;
        const int Deaths = 2;

        public override string Id => "public_confession";
        public override string Name => "Public Confession";
        public override string Description => "Force accused dissidents to confess publicly, crushing unrest through fear.";
        public override string NarrativeText => "They kneel in the square and recite their crimes. Whether the words are true no longer matters.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            true; // TODO: require Tyranny >= 4 AND FearLevel >= 2

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);
        }
    }
}
