using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class SiegeTooltipSystem : MonoBehaviour
    {
        static SiegeTooltip _tooltip;
        static VisualElement _root;
        static VisualElement _target;
        static bool _visible;

        void Awake()
        {
            Install(GetComponent<UIDocument>().rootVisualElement);
        }

        void OnDestroy()
        {
            if (_tooltip != null)
                _tooltip.RemoveFromHierarchy();
        }

        static void Install(VisualElement root)
        {
            if (_tooltip != null)
                _tooltip.RemoveFromHierarchy();

            _root = root;
            _tooltip = new SiegeTooltip();
            root.Add(_tooltip);
            root.RegisterCallback<PointerMoveEvent>(OnPointerMove, TrickleDown.TrickleDown);
        }

        public static void Show(VisualElement target, string title, string description = null, Action<VisualElement> buildContent = null)
        {
            if (_tooltip == null)
                UISystem.Open<SiegeTooltipSystem>(UILayer.Tooltip);

            _target = target;
            _visible = true;
            _tooltip!.Show(title, description, buildContent);
        }

        public static void Hide()
        {
            if (_tooltip == null) return;
            _visible = false;
            _target = null;
            _tooltip.Hide();
        }

        static void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_visible || _tooltip == null) return;
            var panelSize = new Vector2(_root.resolvedStyle.width, _root.resolvedStyle.height);
            var targetRect = _target != null ? _target.worldBound : new Rect(evt.position.x, evt.position.y, 0, 0);
            _tooltip.UpdatePosition(evt.position, panelSize, targetRect);
        }
    }
}