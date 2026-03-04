namespace Siege.Gameplay.Laws
{
    public interface Law
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string NarrativeText => null;
        bool IsEnacted { get; set; }
        double ProductionMultiplier => 1.0;
        double FoodConsumptionMultiplier => 1.0;
        double WaterConsumptionMultiplier => 1.0;
        double SiegeDamageMultiplier => 1.0;
    }
}
