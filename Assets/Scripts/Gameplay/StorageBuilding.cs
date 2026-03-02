
using System.Collections.Generic;
using Siege.Gameplay.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Siege.Gameplay
{
    public class StorageBuilding : MonoBehaviour, IPointerClickHandler
    {
        private readonly List<ResourceQuantity> _resources = new();

        public IReadOnlyList<ResourceQuantity> Resources => _resources;

        public void Add(ResourceType resource, int quantity)
        {
            var index = _resources.FindIndex(r => r.Resource == resource);
            if (index == -1)
            {
                _resources.Add(new ResourceQuantity { Resource = resource, Quantity = quantity });
            }
            else
            {
                var rq = _resources[index];
                rq.Quantity += quantity;
                _resources[index] = rq;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UISystem.Open<StorageBuildingView>(UILayer.Window).Show(this);
        }
    }
}