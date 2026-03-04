using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class OathOfMercyLaw : Law
    {
        const double DailyMorale = 5;
        const double DailySickness = -2;

        public override string Id => "oath_of_mercy";
        public override string Name => "Oath of Mercy";
        public override string Description => "Swear a public oath to protect all citizens. Lifts morale and eases sickness, but slows output.";
        public override string NarrativeText => "We swore to protect them. All of them. Even when it costs us.";

        public override double ProductionMultiplier => 0.9;

        public override bool CanEnact(GameState state) => true;

        protected override void ApplyImmediate(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Morale += DailyMorale;
            log.Record("Morale", DailyMorale, "Oath of Mercy");

            state.Sickness += DailySickness;
            log.Record("Sickness", DailySickness, "Oath of Mercy");
        }
    }
}
