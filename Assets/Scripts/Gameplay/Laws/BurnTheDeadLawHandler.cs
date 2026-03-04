using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class BurnTheDeadLawHandler : ILawHandler
    {
        readonly BurnTheDeadLaw _law;
        readonly IPopupService _popup;

        const double SicknessThreshold = 35;
        const double SicknessReduction = -15;
        const double FuelCost = -2;
        const double MoraleCost = -10;

        public BurnTheDeadLawHandler(BurnTheDeadLaw law, IPopupService popup)
        {
            _law = law;
            _popup = popup;
        }

        public string LawId => _law.Id;

        public bool CanEnact(GameState state) => state.Sickness > SicknessThreshold;

        public void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness += SicknessReduction;
            log.Record("Sickness", SicknessReduction, "Burn the Dead");

            state.Fuel += FuelCost;
            log.Record("Fuel", FuelCost, "Burn the Dead");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Burn the Dead");
            _popup.Open(_law.Name, _law.NarrativeText, log.SliceSince(before));
        }
    }
}
