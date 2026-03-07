using System;
using UnityEngine;
using UnityEngine.InputSystem;
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

        VisualElement _target;
        IVisualElementScheduledItem _updateSchedule;

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

            RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void OnDetach(DetachFromPanelEvent _)
        {
            _updateSchedule?.Pause();
        }

        void UpdatePositionFromMouse()
        {
            var currentPanel = this.panel;
            if (currentPanel == null) return;

            var pointer = Pointer.current;
            if (pointer == null) return;

            var screenPos = pointer.position.ReadValue();
            float scale = currentPanel.scaledPixelsPerPoint;
            var panelPos = new Vector2(screenPos.x / scale, (Screen.height - screenPos.y) / scale);

            var panelSize = new Vector2(parent?.resolvedStyle.width ?? 0, parent?.resolvedStyle.height ?? 0);
            var targetRect = _target?.worldBound ?? new Rect(panelPos.x, panelPos.y, 0, 0);
            UpdatePosition(panelPos, panelSize, targetRect);
        }

        internal void Show(VisualElement target, string title, string description, Action<VisualElement> buildContent)
        {
            _target = target;

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

            if (_updateSchedule == null)
                _updateSchedule = schedule.Execute(UpdatePositionFromMouse).Every(0);
            else
                _updateSchedule.Resume();
        }

        internal void Hide()
        {
            _updateSchedule?.Pause();
            _target = null;
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

            float x = mousePosition.x + offset;

            if (x + tw > panelSize.x - margin)
                x = mousePosition.x - offset - tw;
            if (x < margin)
                x = margin;

            float y = PlaceVertically(mousePosition.y, offset, margin, th, panelSize.y, targetRect);

            style.left = x;
            style.top = y;
        }

        static float PlaceVertically(float mouseY, float offset, float margin, float th, float panelH, Rect target)
        {
            float y = mouseY + offset;
            var tooltipRect = new Rect(0, y, 0, th);

            if (!Overlaps(tooltipRect, target))
                return Mathf.Clamp(y, margin, panelH - th - margin);

            // Below the target
            y = target.yMax + margin;
            if (y + th <= panelH - margin)
                return y;

            // Above the target
            y = target.y - th - margin;
            if (y >= margin)
                return y;

            return margin;
        }

        static bool Overlaps(Rect a, Rect b)
        {
            return a.yMin < b.yMax && a.yMax > b.yMin;
        }
    }
}
