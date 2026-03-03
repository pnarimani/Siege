using Siege.Gameplay.Buildings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Siege.Gameplay
{
    /// <summary>
    /// Handles 3D building selection via raycasting. Attached to the main camera.
    /// Click on a building in the world to select it (opens the building panel).
    /// </summary>
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] LayerMask _buildingLayer = ~0;
        [SerializeField] float _maxRayDistance = 200f;

        Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null) _camera = Camera.main;
        }

        void Update()
        {
            if (Mouse.current == null) return;
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;

            // Don't select if clicking on UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            var mousePos = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, _maxRayDistance, _buildingLayer))
            {
                var building = hit.collider.GetComponentInParent<Building>();
                if (building != null)
                {
                    building.Select();
                }
            }
        }
    }
}
