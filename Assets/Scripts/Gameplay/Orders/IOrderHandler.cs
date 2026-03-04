using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public interface IOrderHandler
    {
        string OrderId { get; }
        bool CanIssue(GameState state);
        void Execute(GameState state, ChangeLog log);
        void OnDayTick(GameState state, ChangeLog log);
    }
}
