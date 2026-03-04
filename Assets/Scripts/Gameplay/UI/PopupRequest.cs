using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public struct PopupRequest
    {
        public string Title;
        public string Narrative;
        public IReadOnlyList<StateChange> Changes;
    }
}
