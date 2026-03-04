namespace Siege.Gameplay.Laws
{
    public class ExtendedShiftsLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "extended_shifts";
        public string Name => "Extended Shifts";
        public string Description => "Mandate longer work hours. Boosts production but grinds workers down.";
        public string NarrativeText => "The hammers do not stop. Neither do the coughs.";

        public double ProductionMultiplier => 1.25;
    }
}
