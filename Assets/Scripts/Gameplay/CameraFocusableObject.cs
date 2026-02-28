using UnityEngine;
using UnityEngine.EventSystems;

namespace Siege.Gameplay
{
    public class CameraFocusableObject : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            FindFirstObjectByType<CameraController>().FocusOn(transform);
        }
    }
}