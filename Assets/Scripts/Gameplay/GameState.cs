namespace Siege.Gameplay
{
    public class GameState
    {
        private readonly GameBalance _gameBalance;

        public double Time { get; set; }
        public int Day => (int)(Time / _gameBalance.DayDurationSeconds);

        public GameState(GameBalance gameBalance)
        {
            _gameBalance = gameBalance;
        }
    }
}