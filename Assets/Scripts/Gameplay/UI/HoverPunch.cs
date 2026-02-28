using System;
using System.Collections.Generic;
using FastSpring;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Siege.Gameplay.UI
{
    public class HoverPunch : MonoBehaviour
    {
        const string HoverPunchClass = "hover-punch";

        readonly List<Action> _updateActions = new();
        readonly List<Action> _interpolateActions = new();
        readonly HashSet<VisualElement> _processedElements = new();

        void Awake()
        {
            var ui = GetComponent<UIDocument>();

            ui.rootVisualElement.Query(className: HoverPunchClass)
                .ForEach(AddHoverPunch);

            ui.rootVisualElement.RegisterCallback<AttachToPanelEvent>(OnAttach);
        }

        void OnAttach(AttachToPanelEvent evt)
        {
            if (evt.target is VisualElement element)
                ProcessElementAndDescendants(element);
        }

        void FixedUpdate()
        {
            foreach (var updateAction in _updateActions)
                updateAction();
        }

        void Update()
        {
            foreach (var interpolateAction in _interpolateActions)
                interpolateAction();
        }

        void ProcessElementAndDescendants(VisualElement element)
        {
            if (!_processedElements.Contains(element))
            {
                if (element.ClassListContains(HoverPunchClass))
                    AddHoverPunch(element);

                _processedElements.Add(element);
            }

            foreach (var child in element.Children())
                ProcessElementAndDescendants(child);
        }

        void AddHoverPunch(VisualElement element)
        {
            var scaleSpring = new Spring<float>();
            scaleSpring.UpdateDefinition(25, 0.5f);
            scaleSpring.MoveInstantly(1);

            var rotationSpring = new Spring<float>();
            rotationSpring.UpdateDefinition(15, 0.5f);
            rotationSpring.MoveInstantly(0);

            _updateActions.Add(scaleSpring.Update);
            _interpolateActions.Add(scaleSpring.Interpolate);

            _updateActions.Add(rotationSpring.Update);
            _interpolateActions.Add(rotationSpring.Interpolate);

            element.schedule.Execute(() =>
            {
                element.style.scale = Vector3.one * scaleSpring.InterpolatedCurrent;
                element.style.rotate = new Rotate(Angle.Degrees(rotationSpring.InterpolatedCurrent));
            }).Every(0);

            element.RegisterCallback<MouseEnterEvent>(_ =>
            {
                scaleSpring.Velocity = 1;
                rotationSpring.Velocity = 40f * (Random.value > 0.5f ? 1 : -1);
            });

            element.RegisterCallback<MouseLeaveEvent>(_ => { scaleSpring.Velocity = 2; });

            element.RegisterCallback<ClickEvent>(_ =>
            {
                scaleSpring.Velocity = 10;
                rotationSpring.Velocity = 80f * (Random.value > 0.5f ? 1 : -1);
            });

            element.RegisterCallback<DetachFromPanelEvent>(_ =>
            {
                _updateActions.Remove(scaleSpring.Update);
                _interpolateActions.Remove(scaleSpring.Interpolate);

                _updateActions.Remove(rotationSpring.Update);
                _interpolateActions.Remove(rotationSpring.Interpolate);
            });
        }
    }
}