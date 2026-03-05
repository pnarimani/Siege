using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public sealed class DialogContent
    {
        public string Title;
        public string Description;
        public IReadOnlyList<StateChange> Changes;
        public ResponseOption[] Responses;
        public Action<int> OnRespond;
        public Action OnDismiss;
    }

    public readonly struct ResponseOption
    {
        public readonly string Label;
        public readonly string Tooltip;

        public ResponseOption(string label, string tooltip = null)
        {
            Label = label;
            Tooltip = tooltip;
        }
    }
}
