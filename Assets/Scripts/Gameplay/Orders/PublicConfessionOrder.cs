using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class PublicConfessionOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        const string Narrative = "They kneel in the square and recite their crimes. Whether the words are true no longer matters.";
        const double UnrestReduction = 20;
        const double MoraleLoss = 10;
        const int Deaths = 2;

        public PublicConfessionOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "public_confession";
        public string Name => "Public Confession";
        public string Description => "Force accused dissidents to confess publicly, crushing unrest through fear.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            _political.Tyranny.Value >= 4 && _political.FearLevel.Value >= 2;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Morale -= MoraleLoss;
            log.Record("Morale", -MoraleLoss, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new PublicConfessionOrder(_popup, _political);
    }
}
