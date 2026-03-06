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
    ///   element.AddManipulator(new TooltipManipulator("Title", "Desc", container => container.Add(new Label("custom"))));
    /// </summary>
    public class TooltipManipulator : Manipulator
    {
        readonly string _title;
        readonly string _description;
        readonly Action<VisualElement> _buildContent;

        public TooltipManipulator(string title, string description = null, Action<VisualElement> buildContent = null)
        {
            _title = title;
            _description = description;
            _buildContent = buildContent;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        void OnMouseEnter(MouseEnterEvent evt) => SiegeTooltipSystem.Show(target, _title, _description, _buildContent);
        void OnMouseLeave(MouseLeaveEvent evt) => SiegeTooltipSystem.Hide();
    }
}
