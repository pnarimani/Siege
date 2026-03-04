using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public class PopupService : IPopupService
    {
        public event Action<PopupRequest> Requested;

        public void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null)
        {
            Requested?.Invoke(new PopupRequest
            {
                Title = title,
                Narrative = narrative,
                Changes = changes
            });
        }
    }
}
