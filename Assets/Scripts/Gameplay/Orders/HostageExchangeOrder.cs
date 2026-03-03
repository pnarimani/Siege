using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrder : Order
    {
        const int Cooldown = 0;
        const double DailyFoodCost = 4;
        const double DailyMedicineCost = 2;
        const double DailyMoraleLoss = 3;
        const int WorkerGainInterval = 2;
        const int WorkerGain = 1;

        int _dayCounter;

        public override string Id => "hostage_exchange";
        public override string Name => "Hostage Exchange";
        public override string Description => "Trade supplies to recover captured citizens. A slow, costly process.";
        public override string NarrativeText => "A figure stumbles through the gate, gaunt and shaking. One more mouth to feed. One more soul saved.";
        public override int CooldownDays => Cooldown;
        public override bool IsToggle => true;

        public override bool CanIssue(GameState state) =>
            state.ZonesLostCount >= 1;

        public override void Execute(GameState state, ChangeLog log)
        {
            _dayCounter = 0;
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Id);

            state.Medicine -= DailyMedicineCost;
            log.Record("Medicine", -DailyMedicineCost, Id);

            state.Morale -= DailyMoraleLoss;
            log.Record("Morale", -DailyMoraleLoss, Id);

            _dayCounter++;
            if (_dayCounter >= WorkerGainInterval)
            {
                _dayCounter = 0;
                state.HealthyWorkers += WorkerGain;
                log.Record("HealthyWorkers", WorkerGain, Id);
            }
        }
    }
}
