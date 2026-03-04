namespace Siege.Gameplay.Laws
{
    public class WaterRationingLaw : Law
    {
        public bool IsEnacted { get; set; }
        public string Id => "water_rationing";
        public string Name => "Water Rationing";
        public string Description => "Restrict water usage to essential needs only. Preserves reserves but breeds disease and discontent.";
        public string NarrativeText => "The wells are guarded now. One cup per person. No exceptions.";

        public double WaterConsumptionMultiplier => 0.75;
    }
}
