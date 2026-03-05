using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class DayOfRemembranceOrder : IOrder
    {
        const string Narrative = "Names are read aloud until the sun sets. The living weep for the dead and find strength in grief.";
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 5;
        const double MoraleThreshold = 30;

        readonly IPopupService _popup;

        public DayOfRemembranceOrder(IPopupService popup)
        {
            _popup = popup;
        }

        public string Id => "day_of_remembrance";
        public string Name => "Day of Remembrance";
        public string Description => "Honor the fallen with a day of mourning. Lifts morale but gatherings spread illness.";
        public int CooldownDays => 10;

        public bool CanIssue(GameState state) =>
            state.Morale < MoraleThreshold;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new DayOfRemembranceOrder(_popup);
    }
}
