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

        public GameState(GameBalance gameBalance)
        {
            Current = this;
            _gameBalance = gameBalance;
            
            Morale = gameBalance.StartingMorale;
            Unrest = gameBalance.StartingUnrest;
            Sickness = gameBalance.StartingSickness;
            SiegeIntensity = 1;
        }
    }
}