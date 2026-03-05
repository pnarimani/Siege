using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public interface IPopupService
    {
        void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null);
    }
}
