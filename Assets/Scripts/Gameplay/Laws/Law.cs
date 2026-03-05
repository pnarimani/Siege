using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Laws
{
    [RegisterTypeLookup]
    public interface ILaw
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool CanEnact(GameState state);
        void OnEnact(GameState state, ChangeLog log);
        void ApplyDailyEffect(GameState state, ChangeLog log) { }
        ILaw Clone();
    }
}
