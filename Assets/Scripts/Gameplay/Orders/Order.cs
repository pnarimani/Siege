using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public interface IOrder
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int CooldownDays { get; }
        bool IsToggle => false;
        bool CanDeactivate => true;
        bool CanIssue(GameState state);
        void OnExecute(GameState state, ChangeLog log);
        void ApplyDailyEffect(GameState state, ChangeLog log) { }
        IOrder Clone();
    }
}
