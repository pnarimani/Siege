using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class BurnSurplusOrder : Order
    {
        const int Cooldown = 3;
        const double MaterialsCost = 10;
        const double SicknessReduction = 8;
        const double MoraleGain = 8;

        public override string Id => "burn_surplus";
        public override string Name => "Burn Surplus";
        public override string Description => "Burn contaminated materials to cleanse the area, reducing sickness and lifting spirits.";
        public override string NarrativeText => "The pyre burns high. The stench of rot gives way to clean smoke. People breathe a little easier.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
        }
    }
}
