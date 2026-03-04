using JetBrains.Annotations;
using Siege.Gameplay.Simulation;
using TypeRegistry;

namespace Siege.Gameplay.Laws
{
    [UsedImplicitly]
    [RegisterTypeLookup]
    public interface ILawHandler
    {
        string LawId { get; }
        bool CanEnact(GameState state);
        void ApplyImmediate(GameState state, ChangeLog log);
        void OnDayTick(GameState state, ChangeLog log) { }
    }
}
