using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class SiegeTooltipSystem : MonoBehaviour
    {
        static SiegeTooltip _tooltip;
        static VisualElement _root;
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

        public static void Show(string title, string description = null, Action<VisualElement> buildContent = null)
        {
            if (_tooltip == null)
                UISystem.Open<SiegeTooltipSystem>(UILayer.Tooltip);

            _visible = true;
            _tooltip!.Show(title, description, buildContent);
        }

        public static void Hide()
        {
            if (_tooltip == null) return;
            _visible = false;
            _tooltip.Hide();
        }

        static void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_visible || _tooltip == null) return;
            var panelSize = new Vector2(_root.resolvedStyle.width, _root.resolvedStyle.height);
            _tooltip.UpdatePosition(evt.position, panelSize);
        }
    }
}