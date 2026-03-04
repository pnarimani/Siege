namespace Siege.Gameplay.Laws
{
    public class StrictRationsLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "strict_rations";
        public string Name => "Strict Rations";
        public string Description => "Cut food portions to stretch supplies. Hunger gnaws, but stores last longer.";
        public string NarrativeText => "Half a bowl. That is what each person gets. Half a bowl, and silence.";

        public double FoodConsumptionMultiplier => 0.75;
    }
}
