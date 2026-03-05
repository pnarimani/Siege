using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class SacrificeSickOrder : IOrder
    {
        const string Narrative = "They are taken beyond the walls at night. No one speaks of it in the morning.";
        const int SickRemoved = 3;
        const double SicknessReduction = 8;
        const double UnrestIncrease = 12;
        const double MoraleLoss = 10;
        const int MinSick = 5;

        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public SacrificeSickOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "sacrifice_sick";
        public string Name => "Sacrifice the Sick";
        public string Description => "Remove the terminally ill to slow the spread of disease. A decision no one will forget.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.SickWorkers > MinSick && _political.Tyranny.Value >= 3;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new SacrificeSickOrder(_popup, _political);
    }
}
