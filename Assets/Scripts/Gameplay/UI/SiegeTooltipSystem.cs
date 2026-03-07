using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class SiegeTooltipSystem : MonoBehaviour
    {
        SiegeTooltip _tooltip;

        void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _tooltip = new SiegeTooltip();
            root.Add(_tooltip);
        }

        public static void Show(VisualElement target, string title, string description = null,
            Action<VisualElement> buildContent = null)
        {
            Get().ShowInternal(target, title, description, buildContent);
        }

        static SiegeTooltipSystem Get()
        {
            return UISystem.GetOrOpen<SiegeTooltipSystem>(UILayer.Tooltip);
        }

        void ShowInternal(VisualElement target, string title, string description, Action<VisualElement> buildContent)
        {
            if (_tooltip == null)
                UISystem.Open<SiegeTooltipSystem>(UILayer.Tooltip);

            _tooltip!.Show(target, title, description, buildContent);
        }

        public static void Hide()
        {
            Get().HideInternal();
        }

        void HideInternal()
        {
            _tooltip?.Hide();
        }
    }
}