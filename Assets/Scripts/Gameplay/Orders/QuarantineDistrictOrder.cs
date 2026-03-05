using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class QuarantineDistrictOrder : IOrder
    {
        readonly IPopupService _popup;

        const string Narrative = "The barricades go up. Behind them, the coughing continues — but it does not spread.";
        const double SicknessReduction = 12;
        const double UnrestReduction = 3;
        const double SicknessThreshold = 30;

        public QuarantineDistrictOrder(IPopupService popup) => _popup = popup;

        public string Id => "quarantine_district";
        public string Name => "Quarantine District";
        public string Description => "Seal off a diseased area to contain the outbreak.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new QuarantineDistrictOrder(_popup);
    }
}
