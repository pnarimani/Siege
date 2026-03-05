using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class CrackdownPatrolsOrder : IOrder
    {
        const string Narrative = "The patrols move through the district at midnight. By morning, the streets are quiet. Three bodies are found.";
        const double UnrestReduction = 25;
        const int Deaths = 3;
        const double MoraleLoss = 15;
        const double UnrestThreshold = 40;

        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public CrackdownPatrolsOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "crackdown_patrols";
        public string Name => "Crackdown Patrols";
        public string Description => "Send armed patrols to brutally suppress unrest. Effective but costly in lives and morale.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Unrest > UnrestThreshold && _political.Tyranny.Value >= 1;

        public void OnExecute(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new CrackdownPatrolsOrder(_popup, _political);
    }
}
