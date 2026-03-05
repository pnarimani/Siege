using System;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class MartialLawLaw : ILaw
    {
        const string Narrative = "The council chamber is empty. The gallows are not.";

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public MartialLawLaw(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "martial_law";
        public string Name => "Martial Law";
        public string Description => "Suspend all civil authority. The guards rule now. Unrest is crushed, but at a terrible human cost.";

        public bool CanEnact(GameState state) =>
            state.Unrest > 75 && !state.EnactedLawIds.Contains("curfew");

        public void OnEnact(GameState state, ChangeLog log)
        {
            var before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            if (state.Unrest > 60)
            {
                var delta = state.Unrest - 60;
                state.Unrest = 60;
                log.Record("Unrest", -delta, Name);
            }

            if (state.Morale > 35)
            {
                var delta = state.Morale - 35;
                state.Morale = 35;
                log.Record("Morale", -delta, Name);
            }

            int executions = Math.Min(2, state.HealthyWorkers);
            if (executions > 0)
            {
                state.HealthyWorkers -= executions;
                state.DeathsToday += executions;
                state.TotalDeaths += executions;
                log.Record("HealthyWorkers", -executions, Name);
                log.Record("Deaths", executions, Name);
            }

            _ledger.Withdraw(ResourceType.Food, 10);
            log.Record("Food", -10, Name);
        }

        public ILaw Clone() => new MartialLawLaw(_popup, _ledger);
    }
}
