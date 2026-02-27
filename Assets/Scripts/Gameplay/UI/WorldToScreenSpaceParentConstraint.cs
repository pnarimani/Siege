using UnityEngine;

namespace Siege.Gameplay.UI
{
    public class WorldToScreenSpaceParentConstraint : MonoBehaviour
    {
        public Transform Parent;


        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Parent != null)
            {
                var vpPoint = _camera.WorldToViewportPoint(Parent.position);
                var screenSize = new Vector2(Screen.width, Screen.height);
                transform.position = vpPoint * screenSize;
            }
        }
    }
}