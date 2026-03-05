using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class HungerRiotEvent : IGameEvent
    {
        const int FoodLoss = 80;
        const int UnrestIncrease = 15;
        const int MaxGuardsLost = 5;

        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public HungerRiotEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "hunger_riot";
        public string Name => "Hunger Riot";
        public string Description => "Angry mobs force the granary doors. What little remains is trampled and scattered in the chaos.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.ConsecutiveFoodDeficitDays < 2 || state.Unrest <= 50) return false;
            _hasTriggered = true;
            return true;
        }

        public void Execute(GameState state, ChangeLog log)
        {
            _ledger.Withdraw(ResourceType.Food, FoodLoss);
            state.Unrest += UnrestIncrease;
            int guardsLost = System.Math.Min(MaxGuardsLost, state.Guards);
            state.Guards -= guardsLost;
            state.TotalDeaths += guardsLost;
            state.DeathsToday += guardsLost;
            log.Record("Food", -FoodLoss, Name);
            log.Record("Unrest", UnrestIncrease, Name);
            log.Record("Guards", -guardsLost, Name);
            log.Record("TotalDeaths", guardsLost, Name);
            log.Record("DeathsToday", guardsLost, Name);
        }

        public IGameEvent Clone() => new HungerRiotEvent(_ledger);
    }
}
