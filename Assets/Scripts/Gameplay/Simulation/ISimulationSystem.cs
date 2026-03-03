namespace Siege.Gameplay.Simulation
{
    public interface ISimulationSystem
    {
        void Tick(GameState state, float deltaTime);
        void OnDayStart(GameState state, int day) { }
        void OnNightStart(GameState state, int day) { }
    }
}
