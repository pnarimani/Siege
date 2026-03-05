using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicTrialOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const string Narrative = "The verdict was decided before the trial began. Everyone knows it. No one objects.";
        const int Deaths = 2;
        const double UnrestReduction = 5;
        const double MoraleLoss = 10;

        public PublicTrialOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "public_trial";
        public string Name => "Public Trial";
        public string Description => "Hold a public trial to make an example of dissenters.";
        public int CooldownDays => 5;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 2 || _political.Faith.Value >= 2;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new PublicTrialOrder(_popup, _political);
    }
}
