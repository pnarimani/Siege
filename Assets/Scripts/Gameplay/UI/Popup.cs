using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    /// <summary>
    /// Static API for requesting narrative popups from anywhere in the game.
    /// Laws, orders, and missions call Popup.Open(...) directly to trigger a popup.
    /// EventDialog subscribes to Popup.Requested to handle the display.
    /// </summary>
    public static class Popup
    {
        public static event Action<PopupRequest> Requested;

        public static void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null)
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
