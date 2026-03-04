namespace Siege.Gameplay.Laws
{
    public class CollectiveFarmsLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "collective_farms";
        public string Name => "Collective Farms";
        public string Description => "Communalize all food production. Boosts output but breeds resentment among former landowners.";
        public string NarrativeText => "The fields belong to everyone now. Not everyone agrees.";

        public double ProductionMultiplier => 1.3;
    }
}
