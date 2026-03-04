using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public interface IPopupService
    {
        event Action<PopupRequest> Requested;
        void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null);
    }
}
