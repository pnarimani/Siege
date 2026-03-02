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

        internal void UpdatePosition(Vector2 mousePosition, Vector2 panelSize)
        {
            const float offsetX = 16f;
            const float offsetY = 16f;

            float x = mousePosition.x + offsetX;
            float y = mousePosition.y + offsetY;

            if (resolvedStyle.width > 1f)
                x = Mathf.Min(x, panelSize.x - resolvedStyle.width - 4f);
            if (resolvedStyle.height > 1f)
                y = Mathf.Min(y, panelSize.y - resolvedStyle.height - 4f);

            style.left = x;
            style.top = y;
        }
    }
}
