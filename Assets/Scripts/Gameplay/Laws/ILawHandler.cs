using JetBrains.Annotations;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    [UsedImplicitly]
    public interface ILawHandler
    {
        string LawId { get; }
        bool CanEnact(GameState state);
        void ApplyImmediate(GameState state, ChangeLog log);
        void OnDayTick(GameState state, ChangeLog log);
    }
}
