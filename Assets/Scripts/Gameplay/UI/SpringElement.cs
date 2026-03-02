using System;
using System.Diagnostics;
using FastSpring;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace Siege.Gameplay.UI
{
    [UxmlObject]
    [Serializable]
    public partial class SpringDefinition
    {
        [UxmlAttribute] public bool Enabled = true;
        [UxmlAttribute] public float Frequency = 15f;
        [UxmlAttribute] public float DampingRatio = 0.8f;
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

        public Spring<Vector3> Position { get; } = new();

        public Spring<float> Rotation { get; } = new();

        public Spring<Vector3> Scale { get; } = new();

        public SpringElement()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttach);
            RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void OnAttach(AttachToPanelEvent evt)
        {
            Position.UpdateDefinition(PositionSpring.Frequency, PositionSpring.DampingRatio);
            Position.Interpolation = PositionSpring.Interpolation;

            Rotation.UpdateDefinition(RotationSpring.Frequency, RotationSpring.DampingRatio);
            Rotation.Interpolation = RotationSpring.Interpolation;

            Scale.UpdateDefinition(ScaleSpring.Frequency, ScaleSpring.DampingRatio);
            Scale.Interpolation = ScaleSpring.Interpolation;
            Scale.MoveInstantly(new Vector3(1, 1, 1));

            schedule.Execute(UpdateSprings).Every(0);

            FixedUpdateRunner.Add(FixedUpdate);
        }

        void OnDetach(DetachFromPanelEvent evt)
        {
            FixedUpdateRunner.Remove(FixedUpdate);
        }

        public void UpdateSprings()
        {
            if (PositionSpring.Enabled)
            {
                Position.Interpolate();
                style.translate = Position.InterpolatedCurrent;
            }

            if (RotationSpring.Enabled)
            {
                Rotation.Interpolate();
                style.rotate = new StyleRotate(Angle.Degrees(Rotation.InterpolatedCurrent));
            }

            if (ScaleSpring.Enabled)
            {
                Scale.Interpolate();
                style.scale = Scale.InterpolatedCurrent;
            }
        }

        void FixedUpdate()
        {
            if (PositionSpring.Enabled)
                Position.Update();
            if (RotationSpring.Enabled)
                Rotation.Update();
            if (ScaleSpring.Enabled)
                Scale.Update();
        }
    }
}