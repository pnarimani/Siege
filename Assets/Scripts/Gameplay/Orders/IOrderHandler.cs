using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public interface IOrderHandler
    {
        string OrderId { get; }
        bool CanIssue(GameState state);
        void Execute(GameState state, ChangeLog log);
        void OnDayTick(GameState state, ChangeLog log) { }
    }
}
