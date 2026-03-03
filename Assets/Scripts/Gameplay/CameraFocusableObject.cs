using UnityEngine;
using UnityEngine.EventSystems;

namespace Siege.Gameplay
{
    public class CameraFocusableObject : MonoBehaviour, IPointerClickHandler
    {
        CameraController _camera;

        void Awake() => _camera = FindFirstObjectByType<CameraController>();

        public void OnPointerClick(PointerEventData eventData)
        {
            _camera.FocusOn(transform);
        }
    }
}