using System;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    /// <summary>
    /// Attach to any VisualElement to show a SiegeTooltip on hover.
    /// Requires SiegeTooltipSystem.Install() to have been called on the panel root.
    /// 
    /// Usage:
    ///   element.AddManipulator(new TooltipManipulator("Title", "Description"));
    ///   element.AddManipulator(new TooltipManipulator("Title", descriptionProvider: () => GetDynamicDesc()));
    ///   element.AddManipulator(new TooltipManipulator("Title", "Desc", container => container.Add(new Label("custom"))));
    /// </summary>
    public class TooltipManipulator : Manipulator
    {
        readonly string _title;
        readonly string _description;
        readonly Func<string> _descriptionProvider;
        readonly Action<VisualElement> _buildContent;
        readonly Func<bool> _canShow;

        public TooltipManipulator(string title, string description = null, Action<VisualElement> buildContent = null, Func<bool> canShow = null)
        {
            _title = title;
            _description = description;
            _buildContent = buildContent;
            _canShow = canShow;
        }

        public TooltipManipulator(string title, Func<string> descriptionProvider, Action<VisualElement> buildContent = null, Func<bool> canShow = null)
        {
            _title = title;
            _descriptionProvider = descriptionProvider;
            _buildContent = buildContent;
            _canShow = canShow;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter, TrickleDown.TrickleDown);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave, TrickleDown.TrickleDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter, TrickleDown.TrickleDown);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave, TrickleDown.TrickleDown);
        }

        void OnPointerEnter(PointerEnterEvent evt)
        {
            if (_canShow != null && !_canShow()) return;
            var desc = _descriptionProvider != null ? _descriptionProvider() : _description;
            SiegeTooltipSystem.Show(target, _title, desc, _buildContent);
        }

        void OnPointerLeave(PointerLeaveEvent evt) => SiegeTooltipSystem.Hide();
    }
}
