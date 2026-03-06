using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    /// <summary>
    /// Tooltip panel that appears near the cursor. Managed by SiegeTooltipSystem.
    /// Supports a title, multiline description, and arbitrary custom content via a builder callback.
    /// </summary>
    [UxmlElement]
    public partial class SiegeTooltip : VisualElement
    {
        readonly TextElement _title;
        readonly TextElement _description;
        readonly VisualElement _divider;
        readonly VisualElement _customContent;

        public SiegeTooltip()
        {
            pickingMode = PickingMode.Ignore;
            style.position = Position.Absolute;
            style.display = DisplayStyle.None;

            AddToClassList("tooltip");

            _title = new TextElement { pickingMode = PickingMode.Ignore };
            _title.AddToClassList("tooltip__title");
            _title.AddToClassList("text");
            Add(_title);

            _description = new TextElement { pickingMode = PickingMode.Ignore };
            _description.AddToClassList("tooltip__description");
            _description.AddToClassList("text");
            Add(_description);

            _divider = new VisualElement { pickingMode = PickingMode.Ignore };
            _divider.AddToClassList("tooltip__divider");
            Add(_divider);

            _customContent = new VisualElement { pickingMode = PickingMode.Ignore };
            _customContent.AddToClassList("tooltip__content");
            Add(_customContent);
        }

        internal void Show(string title, string description, Action<VisualElement> buildContent)
        {
            _title.text = title ?? "";
            _title.style.display = string.IsNullOrEmpty(title) ? DisplayStyle.None : DisplayStyle.Flex;

            _description.text = description ?? "";
            _description.style.display = string.IsNullOrEmpty(description) ? DisplayStyle.None : DisplayStyle.Flex;

            _customContent.Clear();
            bool hasCustomContent = buildContent != null;
            if (hasCustomContent)
                buildContent(_customContent);

            bool hasPrimaryContent = !string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description);
            _divider.style.display = hasCustomContent && hasPrimaryContent ? DisplayStyle.Flex : DisplayStyle.None;
            _customContent.style.display = hasCustomContent ? DisplayStyle.Flex : DisplayStyle.None;

            style.display = DisplayStyle.Flex;
        }

        internal void Hide()
        {
            style.display = DisplayStyle.None;
            _customContent.Clear();
        }

        internal void UpdatePosition(Vector2 mousePosition, Vector2 panelSize, Rect targetRect)
        {
            const float offset = 16f;
            const float margin = 4f;

            float tw = resolvedStyle.width;
            float th = resolvedStyle.height;
            if (tw <= 1f || th <= 1f)
            {
                style.left = mousePosition.x + offset;
                style.top = mousePosition.y + offset;
                return;
            }

            // Try below-right of cursor first
            float x = mousePosition.x + offset;
            float y = mousePosition.y + offset;

            // Clamp right edge
            if (x + tw > panelSize.x - margin)
                x = mousePosition.x - offset - tw;

            // Clamp left edge
            if (x < margin)
                x = margin;

            // Clamp bottom edge — also avoid covering the target element
            if (y + th > panelSize.y - margin)
                y = targetRect.y - th - margin;

            // If placing above the target still overflows the top, just clamp to top
            if (y < margin)
                y = margin;

            style.left = x;
            style.top = y;
        }
    }
}
