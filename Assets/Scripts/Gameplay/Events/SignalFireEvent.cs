using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SignalFireEvent : IGameEvent
    {
        const int StartDay = 25;
        const double MinKeepIntegrity = 30;
        const int FuelCost = 5;
        const int MaterialCost = 15;
        const int LightingUnrest = 5;

        readonly ResourceLedger _ledger;
        bool _hasTriggered;

        public SignalFireEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "signal_fire";
        public string Name => "Light the Signal Fire";
        public string Description => "The beacon tower still stands. If we light the fires, someone beyond the hills may see — but so will the enemy.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay >= StartDay
                && state.Zones[ZoneId.Keep].Integrity >= MinKeepIntegrity
                && !state.SignalFireLit
                && _ledger.Has(ResourceType.Fuel, FuelCost)
                && _ledger.Has(ResourceType.Materials, MaterialCost))
            {
                _hasTriggered = true;
                return true;
            }
            return false;
        }

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Light the fires",
                    "-5 Fuel, -15 Materials, +5 Unrest. The signal burns."),
                new EventResponse(
                    "Too risky",
                    "The beacon remains dark.")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    _ledger.Withdraw(ResourceType.Fuel, FuelCost);
                    _ledger.Withdraw(ResourceType.Materials, MaterialCost);
                    state.Unrest += LightingUnrest;
                    state.SignalFireLit = true;
                    log.Record("Fuel", -FuelCost, Name);
                    log.Record("Materials", -MaterialCost, Name);
                    log.Record("Unrest", LightingUnrest, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new SignalFireEvent(_ledger);
    }
}
