
using System.Collections.Generic;
using UnityEngine;

namespace Siege.Gameplay
{
    public class ResourceBuilding : MonoBehaviour
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
    }
}