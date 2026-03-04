using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ForcedLaborOrder : Order
    {
        const int Cooldown = 3;
        const double MaterialsGain = 15;
        const double UnrestIncrease = 8;
        const int Deaths = 2;

        public override string Id => "forced_labor";
        public override string Name => "Forced Labor";
        public override string Description => "Press civilians into dangerous labor to gather materials. Not all will survive.";
        public override string NarrativeText => "Under the lash, the rubble is cleared. Two bodies are pulled from the wreckage at dusk.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            Resolver.Resolve<PoliticalState>().Faith.Value < 4;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials += MaterialsGain;
            log.Record("Materials", MaterialsGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
