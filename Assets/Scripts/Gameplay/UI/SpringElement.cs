using System;
using System.Diagnostics;
using FastSpring;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlObject]
    [Serializable]
    public partial class SpringDefinition
    {
        [UxmlAttribute] public bool Enabled = true;
        [UxmlAttribute] public float Frequency = 15f;
        [UxmlAttribute] public float DampingRatio = 0.7f;
        [UxmlAttribute] public float Interpolation = 20;
    }

    [UxmlElement]
    public partial class SpringElement : VisualElement
    {
        [UxmlObjectReference(name = "position")]
        public SpringDefinition PositionSpring { get; set; } = new();

        [UxmlObjectReference(name = "rotation")]
        public SpringDefinition RotationSpring { get; set; } = new();

        [UxmlObjectReference(name = "scale")] public SpringDefinition ScaleSpring { get; set; } = new();

        readonly Spring<Vector3> _position = new();
        readonly Spring<Vector3> _scale = new();
        readonly Spring<float> _rotation = new();

        public SpringElement()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttach);
            RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void OnAttach(AttachToPanelEvent evt)
        {
            _position.UpdateDefinition(PositionSpring.Frequency, PositionSpring.DampingRatio);
            _position.Interpolation = PositionSpring.Interpolation;

            _rotation.UpdateDefinition(RotationSpring.Frequency, RotationSpring.DampingRatio);
            _rotation.Interpolation = RotationSpring.Interpolation;

            _scale.UpdateDefinition(ScaleSpring.Frequency, ScaleSpring.DampingRatio);
            _scale.Interpolation = ScaleSpring.Interpolation;
            _scale.MoveInstantly(new Vector3(1, 1, 1));

            schedule.Execute(UpdateSprings).Every(0);

            FixedUpdateRunner.Add(FixedUpdate);
        }

        void OnDetach(DetachFromPanelEvent evt)
        {
            FixedUpdateRunner.Remove(FixedUpdate);
        }

        void UpdateSprings()
        {
            if (PositionSpring.Enabled)
            {
                _position.Interpolate();
                style.translate = _position.InterpolatedCurrent;
            }

            if (RotationSpring.Enabled)
            {
                _rotation.Interpolate();
                style.rotate = new StyleRotate(Angle.Degrees(_rotation.InterpolatedCurrent));
            }

            if (ScaleSpring.Enabled)
            {
                _scale.Interpolate();
                style.scale = _scale.InterpolatedCurrent;
            }
        }

        void FixedUpdate()
        {
            if (PositionSpring.Enabled)
                _position.Update();
            if (RotationSpring.Enabled)
                _rotation.Update();
            if (ScaleSpring.Enabled)
                _scale.Update();
        }
    }
}