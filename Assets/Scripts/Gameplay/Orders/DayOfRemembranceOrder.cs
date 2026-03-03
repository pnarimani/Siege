using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class DayOfRemembranceOrder : Order
    {
        const int Cooldown = 10;
        const double MoraleGain = 15;
        const double UnrestReduction = 5;
        const double SicknessIncrease = 5;
        const double MoraleThreshold = 30;

        public override string Id => "day_of_remembrance";
        public override string Name => "Day of Remembrance";
        public override string Description => "Honor the fallen with a day of mourning. Lifts morale but gatherings spread illness.";
        public override string NarrativeText => "Names are read aloud until the sun sets. The living weep for the dead and find strength in grief.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Morale < MoraleThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Id);
        }
    }
}
