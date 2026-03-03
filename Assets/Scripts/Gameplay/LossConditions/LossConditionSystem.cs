using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.LossConditions
{
    public class LossConditionSystem : ISimulationSystem
    {
        const double UnrestRevoltThreshold = 90;
        const int BothDeficitDaysThreshold = 3;

        public GameOverReason Result { get; private set; } = GameOverReason.None;
        public GameEndState EndState { get; private set; }

        public event Action<GameEndState> GameOver;

        public void Tick(GameState state, float deltaTime) { }

        public void OnDayStart(GameState state, int day)
        {
            if (Result != GameOverReason.None) return;

            // Keep breached
            if (state.GetZoneIntegrity(ZoneId.Keep) <= 0)
            {
                Trigger(state, GameOverReason.KeepBreached);
                return;
            }

            // Council revolt
            if (state.Unrest > UnrestRevoltThreshold)
            {
                Trigger(state, GameOverReason.CouncilRevolt);
                return;
            }

            // Total collapse: both food AND water at 0 for N consecutive days
            if (state.ConsecutiveBothDeficitDays >= BothDeficitDaysThreshold)
            {
                Trigger(state, GameOverReason.TotalCollapse);
                return;
            }

            // Victory: relief army arrival
            if (state.ReliefArmyDay > 0 && day >= state.ReliefArmyDay)
            {
                Trigger(state, GameOverReason.Victory);
            }
        }

        void Trigger(GameState state, GameOverReason reason)
        {
            Result = reason;
            EndState = GameEndState.Create(state, reason);
            state.IsGameOver = true;
            GameOver?.Invoke(EndState);
        }
    }
}
