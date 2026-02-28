using FastSpring;
using UnityEngine;

namespace Siege.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float _movementSpeed = 5;

        TransformSpring _movement;
        PlayerInputActions _actions;

        void Awake()
        {
            _movement = GetComponent<TransformSpring>();
            _actions = new PlayerInputActions();
            _actions.Enable();
        }

        void Update()
        {
            if (_actions.Player.Move.IsPressed())
            {
                var input = _actions.Player.Move.ReadValue<Vector2>();
                _movement.Position.Target += new Vector3(input.x, 0, input.y) * (_movementSpeed * Time.deltaTime);
            }
        }

        public void FocusOn(Transform tx)
        {
            _movement.Position.Target = tx.position;
        }
    }
}