using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class BurnTheDeadLawHandler : LawHandler<BurnTheDeadLaw>
    {
        const double SicknessThreshold = 35;
        const double SicknessReduction = -15;
        const double FuelCost = -2;
        const double MoraleCost = -10;

        public BurnTheDeadLawHandler(BurnTheDeadLaw law, IPopupService popup) : base(law, popup) { }

        public override bool CanEnact(GameState state) => state.Sickness > SicknessThreshold;

        public override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness += SicknessReduction;
            log.Record("Sickness", SicknessReduction, "Burn the Dead");

            state.Fuel += FuelCost;
            log.Record("Fuel", FuelCost, "Burn the Dead");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Burn the Dead");
            Popup.Open(Law.Name, Law.NarrativeText, log.SliceSince(before));
        }
    }
}
