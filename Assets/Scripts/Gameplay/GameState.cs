namespace Siege.Gameplay
{
    public class GameState
    {
        readonly GameBalance _gameBalance;

        public static GameState Current { get; private set; }

        public double Time { get; set; }
        public int Day => (int)(Time / _gameBalance.DayDurationSeconds);

        public int Morale { get; set; }

        public int Unrest { get; set; }

        public int Sickness { get; set; }

        public int SiegeIntensity { get; set; }

        public int SiegeEscalationDelayDays { get; set; }

        public int HealthyWorkerCount { get; set; }
        public int SickWorkerCount { get; set; }
        public int TotalWorkerCount => HealthyWorkerCount + SickWorkerCount;
        
        public int ElderlyCount { get; set; }
        
        public int HealthyGuardsCount { get; set; }
        public int SickGuardsCount { get; set; }
        public int TotalGuardsCount => HealthyGuardsCount + SickGuardsCount;
        
        public GameState(GameBalance gameBalance)
        {
            Current = this;
            _gameBalance = gameBalance;

            Morale = gameBalance.StartingMorale;
            Unrest = gameBalance.StartingUnrest;
            Sickness = gameBalance.StartingSickness;
            SiegeIntensity = 1;
            
            HealthyWorkerCount = gameBalance.StartingHealthyWorkers;
            SickWorkerCount = gameBalance.StartingSickWorkers;
            ElderlyCount = gameBalance.StartingElderly;
            HealthyGuardsCount = gameBalance.StartingGuards;
            SickGuardsCount = 0;
        }
    }
}