using Siege.Gameplay.Buildings;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Siege.Gameplay
{
    public class BuildingClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponentInParent<Building>()?.Select();
        }
    }
}
