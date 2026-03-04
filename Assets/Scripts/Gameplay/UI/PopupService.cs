using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public class PopupService : IPopupService
    {
        public void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null)
        {
            Popup.Open(title, narrative, changes);
        }
    }
}
