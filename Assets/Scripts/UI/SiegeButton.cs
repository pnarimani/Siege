using FastSpring;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.UI
{
    [UxmlElement(libraryPath = "")]
    public partial class SiegeButton : Button
    {
        const string SiegeButtonClass = "siege-button";
        
        readonly Spring<float> _scaleSpring;
        readonly Spring<float> _rotationSpring;

        public SiegeButton()
        {
            _scaleSpring = new Spring<float>();
            _scaleSpring.UpdateDefinition(25, 0.5f);
            _scaleSpring.MoveInstantly(1);

            _rotationSpring = new Spring<float>();
            _rotationSpring.UpdateDefinition(15, 0.5f);
            _rotationSpring.MoveInstantly(0);
            
            AddToClassList(SiegeButtonClass);

            RegisterCallback<AttachToPanelEvent>(_ =>
            {
                FixedUpdateRunner.Add(FixedUpdate);
                schedule.Execute(ApplySprings).Every(0);
            });
            RegisterCallback<DetachFromPanelEvent>(_ => FixedUpdateRunner.Remove(FixedUpdate));

            RegisterCallback<MouseEnterEvent>(_ =>
            {
                _scaleSpring.Velocity = 1;
                _rotationSpring.Velocity = 40f * (Random.value > 0.5f ? 1 : -1);
            });
            RegisterCallback<MouseLeaveEvent>(_ => _scaleSpring.Velocity = 2);
            RegisterCallback<ClickEvent>(_ =>
            {
                _scaleSpring.Velocity = 10;
                _rotationSpring.Velocity = 80f * (Random.value > 0.5f ? 1 : -1);
            });
        }

        void FixedUpdate()
        {
            _scaleSpring.Update();
            _rotationSpring.Update();
        }

        void ApplySprings()
        {
            _scaleSpring.Interpolate();
            _rotationSpring.Interpolate();
            style.scale = Vector3.one * _scaleSpring.InterpolatedCurrent;
            style.rotate = new Rotate(Angle.Degrees(_rotationSpring.InterpolatedCurrent));
        }
    }
}