namespace Siege.Gameplay.Laws
{
    public class OathOfMercyLaw : ILaw
    {
        public bool IsEnacted { get; set; }
        public string Id => "oath_of_mercy";
        public string Name => "Oath of Mercy";
        public string Description => "Swear a public oath to protect all citizens. Lifts morale and eases sickness, but slows output.";
        public string NarrativeText => "We swore to protect them. All of them. Even when it costs us.";

        public double ProductionMultiplier => 0.9;
    }
}
