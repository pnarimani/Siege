using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class CrackdownPatrolsOrder : Order
    {
        const int Cooldown = 3;
        const double UnrestReduction = 25;
        const int Deaths = 3;
        const double MoraleLoss = 15;
        const double UnrestThreshold = 40;

        public override string Id => "crackdown_patrols";
        public override string Name => "Crackdown Patrols";
        public override string Description => "Send armed patrols to brutally suppress unrest. Effective but costly in lives and morale.";
        public override string NarrativeText => "The patrols move through the district at midnight. By morning, the streets are quiet. Three bodies are found.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Unrest > UnrestThreshold && Resolver.Resolve<PoliticalState>().Tyranny.Value >= 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
